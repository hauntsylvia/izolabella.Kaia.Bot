using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using Kaia.Bot.Objects.Discord.Commands.Implementations.Intimates.Subs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Intimates
{
    public class Act : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.Act;

        public override List<GuildPermission> RequiredPermissions => new()
        {
        };

        public override string Name => "Act";

        public override string Description => "Perform various actions on users.";

        public override bool GuildsOnly => true;

        public override List<IzolabellaCommandParameter> Parameters => new()
        {
            new("User", "The user to perform this action on.", ApplicationCommandOptionType.User, true)
        };

        public override List<IzolabellaSubCommand> SubCommands => new()
        {
            new Hug(), new Kiss(), new Pat(), new Cuddle()
        };
    }
}
