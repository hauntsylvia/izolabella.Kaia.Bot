using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Self
{
    public class Relationships : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.Relationships;

        public override List<GuildPermission> RequiredPermissions => new();

        public override string Name => "Relationships";

        public override string Description => "View your relationships";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters => new();

        public override List<IIzolabellaCommandConstraint> Constraints => new();

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            await new MyRelationshipsPaginated(Context).StartAsync();
        }
    }
}
