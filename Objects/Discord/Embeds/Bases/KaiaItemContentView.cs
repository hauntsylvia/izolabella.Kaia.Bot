using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public abstract class KaiaItemContentView : IDisposable
    {
        public KaiaItemContentView(KaiaPathEmbedPaginated? PreviousPage, CommandContext Context, bool CanGoBack = false)
        {
            this.PreviousPageIf = PreviousPage;
            this.BackId = $"goback-{IdGenerator.CreateNewId()}";
            this.CanGoBack = CanGoBack;
            this.Context = Context;
            this.Context.Reference.ButtonExecuted += this.CheckForBackButtonAsync;
        }

        public KaiaItemContentView(KaiaItemContentView? PreviousPage, CommandContext Context, bool CanGoBack = false)
        {
            this.PreviousPageElse = PreviousPage;
            this.BackId = $"goback-{IdGenerator.CreateNewId()}";
            this.CanGoBack = CanGoBack;
            this.Context = Context;
            this.Context.Reference.ButtonExecuted += this.CheckForBackButtonAsync;
        }

        private string BackId { get; }

        public bool CanGoBack { get; set; }

        public IEmote GoBackEmote { get; } = Emotes.Embeds.Reverse;

        public CommandContext Context { get; }

        public KaiaPathEmbedPaginated? PreviousPageIf { get; }

        public KaiaItemContentView? PreviousPageElse { get; }

        //public delegate void GoBackHandler(SocketMessageComponent Component);

        //public event GoBackHandler? BackRequested;

        private async Task CheckForBackButtonAsync(SocketMessageComponent Arg)
        {
            if (Arg.IsValidToken && Arg.Data.CustomId == this.BackId && (this.PreviousPageIf != null || this.PreviousPageElse != null) && this.CanGoBack && this.Context.UserContext.User.Id == Arg.User.Id)
            {
                await Arg.DeferAsync();
                await this.ForceBackAsync(this.Context);
            }
        }

        public async Task ForceBackAsync(CommandContext Context)
        {
            if (this.PreviousPageIf != null)
            {
                await this.PreviousPageIf.StartAsync();
            }
            else if (this.PreviousPageElse != null)
            {
                await this.PreviousPageElse.StartAsync(new(Context.UserContext.User.Id));
            }
            this.Dispose();
        }

        public Task<ComponentBuilder> GetDefaultComponents()
        {
            return Task.FromResult(new ComponentBuilder().WithButton("Back", this.BackId, ButtonStyle.Secondary, this.GoBackEmote,
                disabled: !this.CanGoBack || (this.CanGoBack && this.PreviousPageElse == null && this.PreviousPageIf == null)));
        }

        public abstract Task StartAsync(KaiaUser U);

        public abstract Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U);

        public abstract void Dispose();
    }
}
