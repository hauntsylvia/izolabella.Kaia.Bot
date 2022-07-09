using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
{
    public class CreateNewRelationshipView : KaiaItemContentView
    {
        public CreateNewRelationshipView(MyRelationshipsPaginated? Previous, CommandContext Context) : base(Previous, Context, true)
        {
            this.NewRelationship = new(DateTime.UtcNow, string.Empty, Emotes.Counting.Heart, new(), new());
            this.NewRelationship.AddMember(Context.UserContext.User.Id);
            this.InviteUser = new(Context, "Start Inviting", Emotes.Counting.Add, false, false);
            this.InviteUser.OnButtonPush += this.UserInviteAsync;
            this.Done = new(Context, "Done", Emotes.Counting.CheckRare, false, false);
            this.Done.OnButtonPush += this.DoneAsync;
            this.Context.Reference.MessageReceived += this.MessageReceivedAsync;
        }

        public UserRelationship NewRelationship { get; }

        public KaiaButton InviteUser { get; }

        public KaiaButton Done { get; }

        public bool ReceivingUserMention { get; private set; }

        private async Task UserInviteAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            this.ReceivingUserMention = true;
            await Arg.RespondAsync(text: "type the ids of the users to invite, or mention them. u can do this multiple times");
            await DataStores.UserRelationshipsMainDirectory.SaveAsync(this.NewRelationship);
            await this.UpdateEmbedAsync(UserWhoPressed);
        }

        private async Task DoneAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            this.ReceivingUserMention = false;
            await Arg.DeferAsync(true);
            await DataStores.UserRelationshipsMainDirectory.SaveAsync(this.NewRelationship);
            await this.ForceBackAsync(this.Context);
        }

        private async Task MessageReceivedAsync(SocketMessage Arg)
        {
            if (this.ReceivingUserMention &&
                Arg.Author.Id == this.Context.UserContext.User.Id &&
                (ulong.TryParse(Arg.Content, out ulong IdToInv) || Arg.MentionedUsers.Any()))
            {
                if (IdToInv != default && Arg.Author.Id != IdToInv || Arg.MentionedUsers.All(M => M.Id != Arg.Author.Id))
                {
                    if (Arg.MentionedUsers.Count > 0)
                    {
                        foreach (SocketUser? Mentioned in Arg.MentionedUsers)
                        {
                            this.NewRelationship.AddPendingMember(Mentioned.Id);
                        }
                    }
                    else if (IdToInv != default)
                    {
                        this.NewRelationship.AddPendingMember(IdToInv);
                    }
                    await this.StartAsync(new(Arg.Author.Id));
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.MessageReceived -= this.MessageReceivedAsync;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            CreateNewRelationshipViewRaw? A = new(this.Context, this.NewRelationship);
            await A.RefreshAsync();
            return A;
        }

        public async Task<ComponentBuilder> GetComponentsAsync()
        {
            ComponentBuilder CB = await this.GetDefaultComponents();
            CB.WithButton(this.InviteUser.WithDisabled(this.ReceivingUserMention));
            CB.WithButton(this.Done.WithDisabled(false));
            return CB;
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
