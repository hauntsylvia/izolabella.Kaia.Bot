using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Intimates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Intimates.Subs
{
    public class Pat : KaiaSubCommand
    {
        public override string ForeverId => CommandForeverIds.Acts.Pat;

        public override List<GuildPermission> RequiredPermissions => new Act().RequiredPermissions;

        public override string Name => "Pat";

        public override string Description => "Pat someone!";

        public override bool GuildsOnly => true;

        public override List<IzolabellaCommandParameter> Parameters => new Act().Parameters;

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? OtherUser = Arguments.FirstOrDefault(A => A.Name == "user");
            if (OtherUser != null && OtherUser.Value is IUser U)
            {

                IntimateDisplay D = new(new("pat", "pats"), new(Context.UserContext.User.Id), new(U.Id));
                await Context.UserContext.RespondAsync(embed: D.Build());
            }
        }
    }
}
