using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class NutAndBolt : KaiaInventoryItem
    {
        public NutAndBolt() : base(DisplayName: Strings.ItemStrings.NutAndBolt.Name,
                             Description: "a nut and bolt, often found in factories or warehouses.",
                             MarketCost: 0.59,
                             CanInteractWithDirectly: false,
                             KaiaDisplaysThisOnTheStore: false,
                             UsersCanSellThis: true,
                             DisplayEmoteName: Emotes.Items.NutAndBolt)
        {
        }

        public override Task OnKaiaStoreRefresh()
        {
            return Task.CompletedTask;
        }
    }
}
