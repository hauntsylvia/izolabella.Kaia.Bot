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
            this.Leave = new(Context, "Leave", Emotes.Counting.Sub, false, false);
            this.Leave.OnButtonPush += this.LeaveRelationshipAsync;
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
            this.Relationship.RemoveMember(UserWhoPressed.Id);
            if (this.Relationship.KaiaUserIds.Any())
            {
                await DataStores.UserRelationshipsMainDirectory.SaveAsync(this.Relationship);
            }
            else
            {
                await DataStores.UserRelationshipsMainDirectory.DeleteAsync(this.Relationship.Id);
            }
            await Arg.DeferAsync();
            this.Dispose();
            await this.ForceBackAsync(this.Context);
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            RelationshipViewRaw? A = new(this.Relationship);
            await A.RefreshAsync();
            return A;
        }

        public async Task<ComponentBuilder> GetComponentsAsync()
        {
            ComponentBuilder B = await this.GetDefaultComponents();
            return B.WithButton(this.Leave.WithDisabled(!this.Relationship.KaiaUserIds.Any(KUId => KUId == this.Context.UserContext.User.Id)));
        }

        public async Task UpdateEmbedAsync(KaiaUser U)
        {
            Embed E = (await this.GetEmbedAsync(U)).Build();
            MessageComponent Com = (await this.GetComponentsAsync()).Build();
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(components: Com, embed: E);
            }
            else
            {
                await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
                {
                    M.Content = Strings.EmbedStrings.Empty;
                    M.Components = Com;
                    M.Embed = E;
                });
            }
        }

        public override async Task StartAsync(KaiaUser U)
        {
            await this.UpdateEmbedAsync(U);
        }
    }
}
