using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Guilds
{
    public class ViewActiveLocationsCommand : IKaiaCommand
    {
        public string ForeverId => CommandForeverIds.ViewActiveLocations;

        public string Name => "Locations";

        public string Description => "View all active locations.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new();

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            LocationViewPaginated A = new(Context); 
            await A.StartAsync();
        }
    }
}
