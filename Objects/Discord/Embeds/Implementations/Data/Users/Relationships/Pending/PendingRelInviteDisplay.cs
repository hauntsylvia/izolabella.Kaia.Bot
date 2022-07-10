using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.Pending;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.Pending
{
    public class PendingRelInviteDisplay : KaiaItemContentView
    {
        public PendingRelInviteDisplay(PendingRelationshipInvitesPaginated? Pending, CommandContext Context, UserRelationship Rel) : base(Pending, Context, true)
        {
            Accept = new(Context, "Accept", Emotes.Counting.Heart, false, false);
            Decline = new(Context, "Decline", Emotes.Counting.Invalid, false, false);
            Accept.OnButtonPush += AcceptAsync;
            Decline.OnButtonPush += DeclineAsync;
            this.Rel = Rel;
        }

        private async Task DeclineAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            Rel.UserDeclines(UserWhoPressed.Id);
            Dispose();
            await DataStores.UserRelationshipsMainDirectory.SaveAsync(Rel);
            await ForceBackAsync(Context);
        }

        private async Task AcceptAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            if (Rel.AddMember(UserWhoPressed.Id))
            {
                Dispose();
                await Arg.DeferAsync();
                await DataStores.UserRelationshipsMainDirectory.SaveAsync(Rel);
                await ForceBackAsync(Context);
            }
            else
            {
                await Arg.RespondAsync("looks like this relationship is at the max that I can keep up with!");
            }
        }

        public KaiaButton Accept { get; }

        public KaiaButton Decline { get; }

        public UserRelationship Rel { get; }

        public override void Dispose()
        {
            Accept.Dispose();
            Decline.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<ComponentBuilder> GetComponentsAsync()
        {
            ComponentBuilder CB = await GetDefaultComponents();
            CB.WithButton(Accept.WithDisabled(Rel.AtMax));
            CB.WithButton(Decline.WithDisabled(false));
            return CB;
        }

        public override Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            return Task.FromResult<KaiaPathEmbedRefreshable>(new PendingRelInviteDisplayRaw(Rel));
        }

        public override async Task StartAsync(KaiaUser U)
        {
            ComponentBuilder B = await GetComponentsAsync();
            KaiaPathEmbedRefreshable Embed = await GetEmbedAsync(U);
            await Embed.RefreshAsync();
            await Context.UserContext.ModifyOriginalResponseAsync(A =>
            {
                A.Embed = Embed.Build();
                A.Components = B.Build();
            });
        }
    }
}
