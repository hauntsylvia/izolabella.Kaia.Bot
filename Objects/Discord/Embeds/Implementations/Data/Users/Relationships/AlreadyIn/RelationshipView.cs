using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
{
    public class RelationshipView : KaiaItemContentView
    {
        public RelationshipView(MyRelationshipsPaginated? Previous, CommandContext Context, UserRelationship Relationship) : base(Previous, Context, true)
        {
            this.Relationship = Relationship;
            Leave = new(Context, "Leave", Emotes.Counting.Sub, false, false);
            Leave.OnButtonPush += LeaveRelationshipAsync;
        }

        public UserRelationship Relationship { get; }

        public KaiaButton Leave { get; }

        public bool ReceivingUserMention { get; private set; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async Task LeaveRelationshipAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            Relationship.RemoveMember(UserWhoPressed.Id);
            if (Relationship.KaiaUserIds.Any())
            {
                await DataStores.UserRelationshipsMainDirectory.SaveAsync(Relationship);
            }
            else
            {
                await DataStores.UserRelationshipsMainDirectory.DeleteAsync(Relationship.Id);
            }
            await Arg.DeferAsync();
            Dispose();
            await ForceBackAsync(Context);
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            RelationshipViewRaw? A = new(Relationship);
            await A.RefreshAsync();
            return A;
        }

        public async Task<ComponentBuilder> GetComponentsAsync()
        {
            ComponentBuilder B = await GetDefaultComponents();
            return B.WithButton(Leave.WithDisabled(!Relationship.KaiaUserIds.Any(KUId => KUId == Context.UserContext.User.Id)));
        }

        public async Task UpdateEmbedAsync(KaiaUser U)
        {
            Embed E = (await GetEmbedAsync(U)).Build();
            MessageComponent Com = (await GetComponentsAsync()).Build();
            if (!Context.UserContext.HasResponded)
            {
                await Context.UserContext.RespondAsync(components: Com, embed: E);
            }
            else
            {
                await Context.UserContext.ModifyOriginalResponseAsync(M =>
                {
                    M.Content = Strings.EmbedStrings.Empty;
                    M.Components = Com;
                    M.Embed = E;
                });
            }
        }

        public override async Task StartAsync(KaiaUser U)
        {
            await UpdateEmbedAsync(U);
        }
    }
}
