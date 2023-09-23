using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Exploration
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