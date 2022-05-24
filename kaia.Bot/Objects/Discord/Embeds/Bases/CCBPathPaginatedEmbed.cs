using Discord;
using Discord.Rest;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Clients;
using Kaia.Bot.Objects.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    internal class CCBPathPaginatedEmbed : CCBPathEmbed, IDisposable
    {
        public CCBPathPaginatedEmbed(
            List<CCBPathEmbed> Embeds,
            CommandContext Context,
            int StartingIndex,
            IEmote PageBack, 
            IEmote PageForward,
            string Parent,
            string? Sub1 = null,
            string? Sub2 = null,
            Color? Override = null) : base(Parent, Sub1, Sub2, Override)
        {
            this.Embeds = Embeds;
            this.Context = Context;
            this.ZeroBasedIndex = StartingIndex;
            this.PageBack = PageBack;
            this.PageForward = PageForward;
            this.Parent = Parent;
            this.Sub1 = Sub1;
            this.Sub2 = Sub2;
            this.Override = Override;
            this.BId = $"back-paginationembed-{IdGenerator.CreateNewId()}";
            this.FId = $"forward-paginationembed-{IdGenerator.CreateNewId()}";
        }

        public List<CCBPathEmbed> Embeds { get; }

        public CommandContext Context { get; }

        private int index;

        private string FId { get; }
        private string BId { get; }

        public int ZeroBasedIndex
        {
            get => this.index >= this.Embeds.Count ? this.Embeds.Count - 1 : this.index < 0 ? 0 : this.index;
            set => this.index = value;
        }
        public IEmote PageBack { get; }
        public IEmote PageForward { get; }
        public string Parent { get; }
        public string? Sub1 { get; }
        public string? Sub2 { get; }
        public Color? Override { get; }

        public delegate void PageChangeHandler(CCBPathEmbed Page, int ZeroBasedIndex);

        public event PageChangeHandler? OnPageChange;

        private ComponentBuilder GetComponentBuilder()
        {
            return new ComponentBuilder().WithButton(emote: this.PageBack,
                                                     customId: this.BId,
                                                     disabled: this.ZeroBasedIndex <= 0, 
                                                     style: ButtonStyle.Secondary)
                                         .WithButton(emote: this.PageForward,
                                                     customId: this.FId,
                                                     disabled: this.ZeroBasedIndex >= this.Embeds.Count - 1,
                                                     style: ButtonStyle.Secondary);
        }

        public async Task StartAsync()
        {
            if(!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            await this.Context.UserContext.ModifyOriginalResponseAsync(SelfMessageAction =>
            {
                SelfMessageAction.Content = Strings.EmbedStrings.Empty;
                SelfMessageAction.Components = this.GetComponentBuilder().Build();
                SelfMessageAction.Embed =
                    this.Embeds.ElementAtOrDefault(this.ZeroBasedIndex) is CCBPathEmbed Embed ? Embed.Build() :
                        this.Embeds.ElementAt(this.ZeroBasedIndex >= this.Embeds.Count ? this.Embeds.Count - 1 : 0).Build();
            });
            this.Context.Reference.Client.ButtonExecuted += this.ClientButtonPressedAsync;
        }

        private async Task ClientButtonPressedAsync(global::Discord.WebSocket.SocketMessageComponent Component)
        {
            if (Component.Data.CustomId == this.BId || Component.Data.CustomId == this.FId)
            {
                this.ZeroBasedIndex = Component.Data.CustomId == this.BId ? this.ZeroBasedIndex - 1 : this.ZeroBasedIndex + 1;
                CCBPathEmbed EmbedOfThis = this.Embeds.ElementAt(this.ZeroBasedIndex);
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
            this.Context.Reference.Client.ButtonExecuted -= this.ClientButtonPressedAsync;
        }
    }
}
