using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class Cigarette : KaiaInventoryItem
    {
        public Cigarette() : base(DisplayName: Strings.ItemStrings.Cigarette.Name,
                             Description: "a cigarette.",
                             MarketCost: 12.99,
                             CanInteractWithDirectly: true,
                             KaiaDisplaysThisOnTheStore: false,
                             UsersCanSellThis: true,
                             DisplayEmoteName: Emotes.Items.Cigarette)
        {
            this.OnInteract = new(Strings.ItemStrings.Cigarette.Message, new(-5));
        }

        public override Task OnKaiaStoreRefresh()
        {
            this.OnInteract = new(Strings.ItemStrings.Cigarette.Message, new(-5));
            return Task.CompletedTask;
        }
    }
}
