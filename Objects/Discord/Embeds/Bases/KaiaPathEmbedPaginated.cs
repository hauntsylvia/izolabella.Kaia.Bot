using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Util;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases
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
            IfNoListElements = new ListOfItemsNotFound();
            this.Context = Context;
            ZeroBasedIndex = StartingIndex;
            this.Parent = Parent;
            this.Sub1 = Sub1;
            this.Sub2 = Sub2;
            this.Override = Override;
            this.Private = Private;
            BId = $"back-paginationembed-{IdGenerator.CreateNewId()}";
            FId = $"forward-paginationembed-{IdGenerator.CreateNewId()}";
            GlobalSelectMenuId = IdGenerator.CreateNewId();
        }

        public Dictionary<KaiaPathEmbedRefreshable, List<SelectMenuOptionBuilder>?> EmbedsAndOptions { get; }

        public KaiaPathEmbedRefreshable IfNoListElements { get; }

        public CommandContext Context { get; }

        private int index;

        public int ZeroBasedIndex
        {
            get => index >= EmbedsAndOptions.Count && EmbedsAndOptions.Count > 0 ? EmbedsAndOptions.Count - 1 : index < 0 ? 0 : index;
            set => index = value;
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
            List<SelectMenuOptionBuilder>? B = EmbedsAndOptions.ElementAtOrDefault(ZeroBasedIndex).Value;
            ComponentBuilder CB = new();
            if (B != null)
            {
                CB.WithSelectMenu(menu: new(GetIdFromIndex(), EmbedsAndOptions.ElementAtOrDefault(ZeroBasedIndex).Value), row: 0);
            }

            CB.WithButton(emote: PageBack,
                          customId: BId,
                          disabled: ZeroBasedIndex <= 0,
                          style: ButtonStyle.Secondary,
                          row: 0).WithButton(emote: PageForward,
                                             customId: FId,
                                             disabled: ZeroBasedIndex >= EmbedsAndOptions.Count - 1,
                                             style: ButtonStyle.Secondary,
                                             row: 0);
            IEnumerable<KaiaButton>? Buttons = GetExtraComponentsAsync().Result;
            if (Buttons != null)
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
            return $"selmenuspg-{GlobalSelectMenuId}-{IndexOverride ?? ZeroBasedIndex}";
        }

        public async Task StartAsync()
        {
            if (Context.UserContext.IsValidToken)
            {
                await RefreshAsync();
                MessageComponent Comps = GetComponentBuilder().Build();
                KaiaPathEmbedRefreshable RefreshableEmbed = EmbedsAndOptions.ElementAtOrDefault(ZeroBasedIndex).Key is KaiaPathEmbedRefreshable Embed ? Embed :
                            EmbedsAndOptions.ElementAtOrDefault(ZeroBasedIndex >= EmbedsAndOptions.Count ? EmbedsAndOptions.Count - 1 : 0).Key ?? IfNoListElements;
                Embed BuiltEmbed = await GetEmbedAsync(RefreshableEmbed);
                if (!Context.UserContext.HasResponded)
                {
                    await Context.UserContext.RespondAsync(
                        components: Comps,
                        embed: BuiltEmbed, ephemeral: Private);
                }
                else
                {
                    await Context.UserContext.ModifyOriginalResponseAsync(SelfMessageAction =>
                    {
                        SelfMessageAction.Components = Comps;
                        SelfMessageAction.Embed = BuiltEmbed;
                    });
                }
                Context.Reference.ButtonExecuted += ClientButtonPressedAsync;
                Context.Reference.SelectMenuExecuted += ClientSelectMenuExecutedAsync;
            }
        }

        private Task ClientSelectMenuExecutedAsync(SocketMessageComponent Component)
        {
            if (Component.IsValidToken && Component.Data.CustomId == GetIdFromIndex() && Component.User.Id == Context.UserContext.User.Id)
            {
                KaiaPathEmbedRefreshable EmbedOfThis = EmbedsAndOptions.ElementAt(ZeroBasedIndex).Key;
                //await EmbedOfThis.ClientRefreshAsync();
                ItemSelected?.Invoke(EmbedOfThis, ZeroBasedIndex, Component, Component.Data.Values);
            }
            return Task.CompletedTask;
        }

        private async Task ClientButtonPressedAsync(SocketMessageComponent Component)
        {
            if ((Component.Data.CustomId == BId || Component.Data.CustomId == FId) && Component.User.Id == Context.UserContext.User.Id)
            {
                await RefreshAsync();
                ZeroBasedIndex = Component.Data.CustomId == BId ? ZeroBasedIndex - 1 : ZeroBasedIndex + 1;
                KaiaPathEmbedRefreshable RefreshableEmbed = EmbedsAndOptions.ElementAt(ZeroBasedIndex).Key;
                Embed BuiltEmbed = await GetEmbedAsync(RefreshableEmbed);
                await Component.UpdateAsync(M =>
                {
                    M.Content = Strings.EmbedStrings.Empty;
                    M.Embed = BuiltEmbed;
                    M.Components = GetComponentBuilder().Build();
                });
                OnPageChange?.Invoke(RefreshableEmbed, ZeroBasedIndex);
            }
        }

        public void Dispose()
        {
            Context.Reference.ButtonExecuted -= ClientButtonPressedAsync;
            Context.Reference.SelectMenuExecuted -= ClientSelectMenuExecutedAsync;
            GC.SuppressFinalize(this);
        }

        public async Task RefreshAsync()
        {
            EmbedsAndOptions.Clear();
            await ClientRefreshAsync();
        }

        public virtual Task<IEnumerable<KaiaButton>?> GetExtraComponentsAsync()
        {
            return Task.FromResult<IEnumerable<KaiaButton>?>(null);
        }

        protected abstract Task ClientRefreshAsync();
    }
}
