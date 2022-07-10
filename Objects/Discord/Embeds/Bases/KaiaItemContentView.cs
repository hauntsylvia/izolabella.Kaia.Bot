using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public abstract class KaiaItemContentView : IDisposable
    {
        public KaiaItemContentView(KaiaPathEmbedPaginated? PreviousPage, CommandContext Context, bool CanGoBack = false)
        {
            PreviousPageIf = PreviousPage;
            BackId = $"goback-{IdGenerator.CreateNewId()}";
            this.CanGoBack = CanGoBack;
            this.Context = Context;
            this.Context.Reference.ButtonExecuted += CheckForBackButtonAsync;
        }

        public KaiaItemContentView(KaiaItemContentView? PreviousPage, CommandContext Context, bool CanGoBack = false)
        {
            PreviousPageElse = PreviousPage;
            BackId = $"goback-{IdGenerator.CreateNewId()}";
            this.CanGoBack = CanGoBack;
            this.Context = Context;
            this.Context.Reference.ButtonExecuted += CheckForBackButtonAsync;
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
            if (Arg.IsValidToken && Arg.Data.CustomId == BackId && (PreviousPageIf != null || PreviousPageElse != null) && CanGoBack && Context.UserContext.User.Id == Arg.User.Id)
            {
                await Arg.DeferAsync();
                await ForceBackAsync(Context);
            }
        }

        public async Task ForceBackAsync(CommandContext Context)
        {
            if (PreviousPageIf != null)
            {
                await PreviousPageIf.StartAsync();
            }
            else if (PreviousPageElse != null)
            {
                await PreviousPageElse.StartAsync(new(Context.UserContext.User.Id));
            }
            Dispose();
        }

        public Task<ComponentBuilder> GetDefaultComponents()
        {
            return Task.FromResult(new ComponentBuilder().WithButton("Back", BackId, ButtonStyle.Secondary, GoBackEmote,
                disabled: !CanGoBack || (CanGoBack && PreviousPageElse == null && PreviousPageIf == null)));
        }

        public abstract Task StartAsync(KaiaUser U);

        public abstract Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U);

        public abstract void Dispose();
    }
}
