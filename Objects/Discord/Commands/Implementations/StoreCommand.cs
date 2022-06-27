using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops;
using izolabella.Util;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class StoreCommand : IKaiaCommand
    {
        public string Name => "Store";

        public string Description => "Display the global store, or buy an item.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new();

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.StoreOrBuy;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            List<KaiaInventoryItem> Items = BaseImplementationUtil.GetItems<KaiaInventoryItem>();
            ItemsPaginated E = new(Context, Items, 6);
            await E.StartAsync();
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
