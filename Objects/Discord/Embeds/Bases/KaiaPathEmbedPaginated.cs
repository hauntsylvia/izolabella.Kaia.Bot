using izolabella.Util;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public class KaiaPathEmbedPaginated : IDisposable
    {
        public KaiaPathEmbedPaginated(
            Dictionary<KaiaPathEmbed, List<SelectMenuOptionBuilder>?> EmbedsAndOptions,
            KaiaPathEmbed IfNoListElements,
            CommandContext Context,
            int StartingIndex,
            string Parent,
            string? Sub1 = null,
            string? Sub2 = null,
            Color? Override = null)
        {
            this.EmbedsAndOptions = EmbedsAndOptions;
            this.IfNoListElements = IfNoListElements;
            this.Context = Context;
            this.ZeroBasedIndex = StartingIndex;
            this.Parent = Parent;
            this.Sub1 = Sub1;
            this.Sub2 = Sub2;
            this.Override = Override;
            this.BId = $"back-paginationembed-{IdGenerator.CreateNewId()}";
            this.FId = $"forward-paginationembed-{IdGenerator.CreateNewId()}";
            this.GlobalSelectMenuId = IdGenerator.CreateNewId();
        }

        public Dictionary<KaiaPathEmbed, List<SelectMenuOptionBuilder>?> EmbedsAndOptions { get; }
        public KaiaPathEmbed IfNoListElements { get; }
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

        public delegate void PageChangeHandler(KaiaPathEmbed Page, int ZeroBasedIndex);

        public event PageChangeHandler? OnPageChange;

        public delegate void ItemSelectedHandler(KaiaPathEmbed Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected);

        public event ItemSelectedHandler? ItemSelected;

        private ComponentBuilder GetComponentBuilder()
        {
            List<SelectMenuOptionBuilder>? B = this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex).Value;
            ComponentBuilder CB = new();
            if (B != null)
            {
                _ = CB.WithSelectMenu(menu: new(this.GetIdFromIndex(), this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex).Value), row: 0);
            }

            _ = CB.WithButton(emote: this.PageBack,
                          customId: this.BId,
                          disabled: this.ZeroBasedIndex <= 0,
                          style: ButtonStyle.Secondary,
                          row: 0).WithButton(emote: this.PageForward,
                                             customId: this.FId,
                                             disabled: this.ZeroBasedIndex >= this.EmbedsAndOptions.Count - 1,
                                             style: ButtonStyle.Secondary,
                                             row: 0);
            return CB;
        }

        private string GetIdFromIndex(int? IndexOverride = null)
        {
            return $"selmenuspg-{this.GlobalSelectMenuId}-{IndexOverride ?? this.ZeroBasedIndex}";
        }

        public async Task StartAsync()
        {
            if (this.Context.UserContext.IsValidToken)
            {
                MessageComponent Comps = this.GetComponentBuilder().Build();
                Embed Emb = this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex).Key is KaiaPathEmbed Embed ? Embed.Build() :
                            this.EmbedsAndOptions.ElementAtOrDefault(this.ZeroBasedIndex >= this.EmbedsAndOptions.Count ? this.EmbedsAndOptions.Count - 1 : 0).Key?.Build() ?? this.IfNoListElements.Build();
                if (!this.Context.UserContext.HasResponded)
                {
                    await this.Context.UserContext.RespondAsync(
                        components: Comps,
                        embed: Emb);
                }
                else
                {
                    _ = await this.Context.UserContext.ModifyOriginalResponseAsync(SelfMessageAction =>
                    {
                        SelfMessageAction.Components = Comps;
                        SelfMessageAction.Embed = Emb;
                    });
                }
                this.Context.Reference.Client.ButtonExecuted += this.ClientButtonPressedAsync;
                this.Context.Reference.Client.SelectMenuExecuted += this.ClientSelectMenuExecutedAsync;
            }
        }

        private Task ClientSelectMenuExecutedAsync(SocketMessageComponent Component)
        {
            if (Component.IsValidToken && Component.Data.CustomId == this.GetIdFromIndex() && Component.User.Id == this.Context.UserContext.User.Id)
            {
                KaiaPathEmbed EmbedOfThis = this.EmbedsAndOptions.ElementAt(this.ZeroBasedIndex).Key;
                this.ItemSelected?.Invoke(EmbedOfThis, this.ZeroBasedIndex, Component, Component.Data.Values);
            }
            return Task.CompletedTask;
        }

        private async Task ClientButtonPressedAsync(SocketMessageComponent Component)
        {
            if ((Component.Data.CustomId == this.BId || Component.Data.CustomId == this.FId) && Component.User.Id == this.Context.UserContext.User.Id)
            {
                this.ZeroBasedIndex = Component.Data.CustomId == this.BId ? this.ZeroBasedIndex - 1 : this.ZeroBasedIndex + 1;
                KaiaPathEmbed EmbedOfThis = this.EmbedsAndOptions.ElementAt(this.ZeroBasedIndex).Key;
                await Component.UpdateAsync(M =>
                {
                    M.Content = Strings.EmbedStrings.Empty;
                    M.Embed = EmbedOfThis.Build();
                    M.Components = this.GetComponentBuilder().Build();
                });
                this.OnPageChange?.Invoke(EmbedOfThis, this.ZeroBasedIndex);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.Client.ButtonExecuted -= this.ClientButtonPressedAsync;
            this.Context.Reference.Client.SelectMenuExecuted -= this.ClientSelectMenuExecutedAsync;
        }
    }
}
