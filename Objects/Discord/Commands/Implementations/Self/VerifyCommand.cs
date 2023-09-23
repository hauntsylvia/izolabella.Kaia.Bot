using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Others;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn;
using izolabella.LoFi.Server.Structures.Endpoints;
using izolabella.Music.Structure.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Self
{
    public class VerifyCommand : KaiaCommand
    {
        public override string Name => "LoFi-Verify";

        public override string Description => "Connect your Kaia profile with the LoFi server.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters => new();

        public override List<IIzolabellaCommandConstraint> Constraints => new();

        public override string ForeverId => CommandForeverIds.Verify;

        public override List<GuildPermission> RequiredPermissions => new();

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            KaiaUser U = new(Context.UserContext.User.Id);
            LoFiUser? R = await izolabella.LoFi.Server.Structures.Constants.DataStores.UserStore.ReadAsync<LoFiUser>(Context.UserContext.User.Id);
            VerifyEmbed VE = new(U, R);
            KaiaButton Button = new(Context, VE.VLink, "Verify", Emotes.Counting.CheckRare, R != null, false);
            await VE.RefreshAsync();
            await Context.UserContext.RespondAsync(embed: VE.Build(), ephemeral: true, components: new ComponentBuilder().WithButton(Button).Build());
            Verify.UserVerified += async (DiscordId, LUser) =>
            {
                if (Context.UserContext.IsValidToken)
                {
                    LUser.Profile.DisplayName = Context.UserContext.User.Username;
                    await LoFi.Server.Structures.Constants.DataStores.UserStore.SaveAsync(LUser);
                    VE = new(U, LUser);
                    await VE.RefreshAsync();
                    await Context.UserContext.ModifyOriginalResponseAsync(R =>
                    {
                        R.Components = new ComponentBuilder().Build();
                        R.Embed = VE.Build();
                    });
                }
            };
        }
    }
}