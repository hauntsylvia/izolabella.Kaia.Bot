using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Guilds
{
    public class ViewActiveLocationsCommand : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.ViewActiveLocations;

        public override string Name => "Locations";

        public override string Description => "View all active locations.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters { get; } = new();

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            LocationViewPaginated A = new(Context); 
            await A.StartAsync();
        }
    }
}
