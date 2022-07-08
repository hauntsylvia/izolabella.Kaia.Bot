using izolabella.Util;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public abstract class KaiaPathEmbedPaginated : IDisposable
    {
        public KaiaPathEmbedPaginated(
            Dictionary<KaiaPathEmbedRefreshable, List<SelectMenuOptionBuilder>?> EmbedsAndOptions,
            CommandContext Context,
            int StartingIndex,
            string Parent,
            string? Sub1 = null,
            string? Sub2 = null,
            Color? Override = null,
            bool Private = false)
        {
            this.EmbedsAndOptions = EmbedsAndOptions;
            this.IfNoListElements = new ListOfItemsNotFound();
            this.Context = Context;
            this.ZeroBasedIndex = StartingIndex;
            this.Parent = Parent;
            this.Sub1 = Sub1;
            this.Sub2 = Sub2;
            this.Override = Override;
            this.Private = Private;
            this.BId = $"back-paginationembed-{IdGenerator.CreateNewId()}";
            this.FId = $"forward-paginationembed-{IdGenerator.CreateNewId()}";
            this.GlobalSelectMenuId = IdGenerator.CreateNewId();
        }

        public Dictionary<KaiaPathEmbedRefreshable, List<SelectMenuOptionBuilder>?> EmbedsAndOptions { get; }

        public KaiaPathEmbedRefreshable IfNoListElements { get; }

        public CommandContext Context { get; }

        private int index;

        public int ZeroBasedIndex
        {
            get => this.index >= this.EmbedsAndOptions.Count && this.EmbedsAndOptions.Count > 0 ? this.EmbedsAndOptions.Count - 1 : this.index < 0 ? 0 : this.index;
            set => this.index = value;
        }

        private string FId { get; }

        private string BId { get; }

        private ulong GlobalSelectMenuId { get; }

        public Emoji PageBack { get; } = Emotes.Embeds.Back;

        public Emoji PageForward { get; } = Emotes.Embeds.Forward;

        public string Parent { get; }

        public string? Sub1 { get; }

        public string? Sub2 { get; }

        public Color? Override { get; }

        public bool Private { get; }

        public delegate void PageChangeHandler(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex);

        public event PageChangeHandler? OnPageChange;

        public delegate void ItemSelectedHandler(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected);

        public event ItemSelectedHandler? ItemSelected;

        private ComponentBuilder GetComponentBuilder()
        {
            List<SelectMenuOptionBuilder>? B = this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex).Value;
            ComponentBuilder CB = new();
            if (B != null)
            {
                CB.WithSelectMenu(menu: new(this.GetIdFromIndex(), this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex).Value), row: 0);
            }

            CB.WithButton(emote: this.PageBack,
                          customId: this.BId,
                          disabled: this.ZeroBasedIndex <= 0,
                          style: ButtonStyle.Secondary,
                          row: 0).WithButton(emote: this.PageForward,
                                             customId: this.FId,
                                             disabled: this.ZeroBasedIndex >= this.EmbedsAndOptions.Count - 1,
                                             style: ButtonStyle.Secondary,
                                             row: 0);
            IEnumerable<KaiaButton>? Buttons = this.GetExtraComponentsAsync().Result;
            if(Buttons != null)
            {
                foreach (KaiaButton Button in Buttons)
                {
                    CB.WithButton(Button);
                }
            }
            return CB;
        }

        private static async Task<Embed> GetEmbedAsync(KaiaPathEmbedRefreshable RefreshableEmbed)
        {
            await RefreshableEmbed.RefreshAsync();
            return RefreshableEmbed.Build();
        }

        private string GetIdFromIndex(int? IndexOverride = null)
        {
            return $"selmenuspg-{this.GlobalSelectMenuId}-{IndexOverride ?? this.ZeroBasedIndex}";
        }

        public async Task StartAsync()
        {
            if (this.Context.UserContext.IsValidToken)
            {
                await this.RefreshAsync();
                MessageComponent Comps = this.GetComponentBuilder().Build();
                KaiaPathEmbedRefreshable RefreshableEmbed = this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex).Key is KaiaPathEmbedRefreshable Embed ? Embed :
                            this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex >= this.EmbedsAndOptions.Count ? this.EmbedsAndOptions.Count - 1 : 0).Key ?? this.IfNoListElements;
                Embed BuiltEmbed = await GetEmbedAsync(RefreshableEmbed);
                if (!this.Context.UserContext.HasResponded)
                {
                    await this.Context.UserContext.RespondAsync(
                        components: Comps,
                        embed: BuiltEmbed, ephemeral: this.Private);
                }
                else
                {
                    await this.Context.UserContext.ModifyOriginalResponseAsync(SelfMessageAction =>
                    {
                        SelfMessageAction.Components = Comps;
                        SelfMessageAction.Embed = BuiltEmbed;
                    });
                }
                this.Context.Reference.ButtonExecuted += this.ClientButtonPressedAsync;
                this.Context.Reference.SelectMenuExecuted += this.ClientSelectMenuExecutedAsync;
            }
        }

        private Task ClientSelectMenuExecutedAsync(SocketMessageComponent Component)
        {
            if (Component.IsValidToken && Component.Data.CustomId == this.GetIdFromIndex() && Component.User.Id == this.Context.UserContext.User.Id)
            {
                KaiaPathEmbedRefreshable EmbedOfThis = this.EmbedsAndOptions.ElementAt(this.ZeroBasedIndex).Key;
                //await EmbedOfThis.ClientRefreshAsync();
                this.ItemSelected?.Invoke(EmbedOfThis, this.ZeroBasedIndex, Component, Component.Data.Values);
            }
            return Task.CompletedTask;
        }

        private async Task ClientButtonPressedAsync(SocketMessageComponent Component)
        {
            if ((Component.Data.CustomId == this.BId || Component.Data.CustomId == this.FId) && Component.User.Id == this.Context.UserContext.User.Id)
            {
                await this.RefreshAsync();
                this.ZeroBasedIndex = Component.Data.CustomId == this.BId ? this.ZeroBasedIndex - 1 : this.ZeroBasedIndex + 1;
                KaiaPathEmbedRefreshable RefreshableEmbed = this.EmbedsAndOptions.ElementAt(this.ZeroBasedIndex).Key;
                Embed BuiltEmbed = await GetEmbedAsync(RefreshableEmbed);
                await Component.UpdateAsync(M =>
                {
                    M.Content = Strings.EmbedStrings.Empty;
                    M.Embed = BuiltEmbed;
                    M.Components = this.GetComponentBuilder().Build();
                });
                this.OnPageChange?.Invoke(RefreshableEmbed, this.ZeroBasedIndex);
            }
        }

        public void Dispose()
        {
            this.Context.Reference.ButtonExecuted -= this.ClientButtonPressedAsync;
            this.Context.Reference.SelectMenuExecuted -= this.ClientSelectMenuExecutedAsync;
            GC.SuppressFinalize(this);
        }

        public async Task RefreshAsync()
        {
            this.EmbedsAndOptions.Clear();
            await this.ClientRefreshAsync();
        }

        public virtual Task<IEnumerable<KaiaButton>?> GetExtraComponentsAsync()
        {
            return Task.FromResult<IEnumerable<KaiaButton>?>(null);
        }

        protected abstract Task ClientRefreshAsync();
    }
}
