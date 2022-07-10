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
            NewRelationship = new(DateTime.UtcNow, string.Empty, Emotes.Counting.Heart, new(), new());
            NewRelationship.AddMember(Context.UserContext.User.Id);
            InviteUser = new(Context, "Start Inviting", Emotes.Counting.Add, false, false);
            InviteUser.OnButtonPush += UserInviteAsync;
            Done = new(Context, "Done", Emotes.Counting.CheckRare, false, false);
            Done.OnButtonPush += DoneAsync;
            this.Context.Reference.MessageReceived += MessageReceivedAsync;
        }

        public UserRelationship NewRelationship { get; }

        public KaiaButton InviteUser { get; }

        public KaiaButton Done { get; }

        public bool ReceivingUserMention { get; private set; }

        private async Task UserInviteAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            ReceivingUserMention = true;
            await Arg.RespondAsync(text: "type the ids of the users to invite, or mention them. u can do this multiple times");
            await DataStores.UserRelationshipsMainDirectory.SaveAsync(NewRelationship);
            await UpdateEmbedAsync(UserWhoPressed);
        }

        private async Task DoneAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            ReceivingUserMention = false;
            await Arg.DeferAsync(true);
            await DataStores.UserRelationshipsMainDirectory.SaveAsync(NewRelationship);
            await ForceBackAsync(Context);
        }

        private async Task MessageReceivedAsync(SocketMessage Arg)
        {
            if (ReceivingUserMention &&
                Arg.Author.Id == Context.UserContext.User.Id &&
                (ulong.TryParse(Arg.Content, out ulong IdToInv) || Arg.MentionedUsers.Any()))
            {
                if ((IdToInv != default && Arg.Author.Id != IdToInv) || Arg.MentionedUsers.All(M => M.Id != Arg.Author.Id))
                {
                    if (Arg.MentionedUsers.Count > 0)
                    {
                        foreach (SocketUser? Mentioned in Arg.MentionedUsers)
                        {
                            NewRelationship.AddPendingMember(Mentioned.Id);
                        }
                    }
                    else if (IdToInv != default)
                    {
                        NewRelationship.AddPendingMember(IdToInv);
                    }
                    await StartAsync(new(Arg.Author.Id));
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Context.Reference.MessageReceived -= MessageReceivedAsync;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            CreateNewRelationshipViewRaw? A = new(Context, NewRelationship);
            await A.RefreshAsync();
            return A;
        }

        public async Task<ComponentBuilder> GetComponentsAsync()
        {
            ComponentBuilder CB = await GetDefaultComponents();
            CB.WithButton(InviteUser.WithDisabled(ReceivingUserMention));
            CB.WithButton(Done.WithDisabled(false));
            return CB;
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
