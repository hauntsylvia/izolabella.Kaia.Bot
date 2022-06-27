using izolabella.Util;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public abstract class KaiaItemContentView : IDisposable
    {
        public KaiaItemContentView(KaiaPathEmbedPaginated? PreviousPage, CommandContext Context, bool CanGoBack = false)
        {
            this.PreviousPage = PreviousPage;
            this.BackId = $"goback-{IdGenerator.CreateNewId()}";
            this.CanGoBack = CanGoBack;
            this.Context = Context;
            this.Context.Reference.Client.ButtonExecuted += this.CheckForBackButtonAsync;
        }

        private string BackId { get; }

        public bool CanGoBack { get; set; }

        public IEmote GoBackEmote { get; } = Emotes.Embeds.Reverse;

        public CommandContext Context { get; }

        public KaiaPathEmbedPaginated? PreviousPage { get; }

        //public delegate void GoBackHandler(SocketMessageComponent Component);

        //public event GoBackHandler? BackRequested;

        private async Task CheckForBackButtonAsync(SocketMessageComponent Arg)
        {
            if (Arg.Data.CustomId == this.BackId && this.PreviousPage != null && this.CanGoBack && this.Context.UserContext.User.Id == Arg.User.Id)
            {
                await Arg.DeferAsync();
                await this.PreviousPage.StartAsync();
                this.Dispose();
            }
        }

        public Task<ComponentBuilder> GetDefaultComponents()
        {
            return Task.FromResult(new ComponentBuilder().WithButton("Back", this.BackId, ButtonStyle.Secondary, this.GoBackEmote, disabled: !this.CanGoBack || (this.CanGoBack && this.PreviousPage == null)));
        }

        public abstract Task StartAsync(KaiaUser U);

        public abstract Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U);

        public abstract void Dispose();
    }
}
