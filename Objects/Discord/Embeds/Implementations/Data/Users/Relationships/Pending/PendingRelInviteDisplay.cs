using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn;
using Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.Pending
{
    public class PendingRelInviteDisplay : KaiaItemContentView
    {
        public PendingRelInviteDisplay(PendingRelationshipInvitesPaginated? Pending, CommandContext Context, UserRelationship Rel) : base(Pending, Context, true)
        {
            this.Accept = new(Context, "Accept", Emotes.Counting.Heart, false, false);
            this.Decline = new(Context, "Decline", Emotes.Counting.Invalid, false, false);
            this.Accept.OnButtonPush += this.AcceptAsync;
            this.Decline.OnButtonPush += this.DeclineAsync;
            this.Rel = Rel;
        }

        private async Task DeclineAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            this.Rel.UserDeclines(UserWhoPressed.Id);
            this.Dispose();
            await this.ForceBackAsync(this.Context);
        }

        private async Task AcceptAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            this.Rel.AddMember(UserWhoPressed.Id);
            this.Dispose();
            await this.ForceBackAsync(this.Context);
        }

        public KaiaButton Accept { get; }

        public KaiaButton Decline { get; }

        public UserRelationship Rel { get; }

        public override void Dispose()
        {
            this.Accept.Dispose();
            this.Decline.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = await this.GetDefaultComponents();
            CB.WithButton(this.Accept.WithDisabled(false));
            CB.WithButton(this.Decline.WithDisabled(false));
            return CB;
        }

        public override Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            return Task.FromResult<KaiaPathEmbedRefreshable>(new PendingRelInviteDisplayRaw(this.Rel));
        }

        public override async Task StartAsync(KaiaUser U)
        {
            ComponentBuilder B = await this.GetComponentsAsync(U);
            KaiaPathEmbedRefreshable Embed = await this.GetEmbedAsync(U);
            await Embed.RefreshAsync();
        }
    }
}
