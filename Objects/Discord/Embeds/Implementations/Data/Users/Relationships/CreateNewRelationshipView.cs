using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;
using Kaia.Bot.Objects.KaiaStructures.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships
{
    public class CreateNewRelationshipView : KaiaItemContentView
    {
        public CreateNewRelationshipView(MyRelationshipsPaginated? Previous, CommandContext Context) : base(Previous, Context, true)
        {
            this.NewRelationship = new(DateTime.UtcNow, new(), new());
            this.InviteUser = new(Context, "Invite User", Emotes.Counting.Add, false, false);
            this.InviteUser.OnButtonPush += this.UserInviteAsync;
            this.Context.Reference.Client.MessageReceived += this.MessageReceivedAsync;
        }

        public UserRelationship NewRelationship { get; }

        public KaiaButton InviteUser { get; }

        public bool ReceivingUserMention { get; }

        private Task UserInviteAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {

            throw new NotImplementedException();
        }

        private async Task MessageReceivedAsync(SocketMessage Arg)
        {
            if (this.ReceivingUserMention && 
                Arg.Author.Id == this.Context.UserContext.User.Id && 
                (ulong.TryParse(Arg.Content, out ulong IdToInv) || Arg.MentionedUsers.Any()))
            {
                if((IdToInv != default && Arg.Author.Id != IdToInv) || Arg.MentionedUsers.All(M => M.Id != Arg.Author.Id))
                {
                    if(Arg.MentionedUsers.Count > 0)
                    {
                        foreach(SocketUser? Mentioned in Arg.MentionedUsers)
                        {
                            this.NewRelationship.AddPendingMember(Mentioned.Id);
                        }
                    }
                    else if(IdToInv != default)
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
            this.Context.Reference.Client.MessageReceived -= this.MessageReceivedAsync;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            CreateNewRelationshipViewRaw? A = new(this.Context, this.NewRelationship);
            await A.RefreshAsync();
            return A;
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = await this.GetDefaultComponents();
            CB.WithButton(this.InviteUser.WithDisabled(false));
            return CB;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            Embed E = (await this.GetEmbedAsync(U)).Build();
            MessageComponent Com = (await this.GetComponentsAsync(U)).Build();
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
    }
}
