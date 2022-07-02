namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class Candle : KaiaInventoryItem
    {
        public Candle() : base(DisplayName: Strings.ItemStrings.Candle.Name,
                             Description: "a candle, both pretty and useful . . at the right time.",
                             MarketCost: 3.78,
                             CanInteractWithDirectly: false,
                             KaiaDisplaysThisOnTheStore: false,
                             UsersCanSellThis: true,
                             DisplayEmoteName: Emotes.Items.Candle)
        {
        }

        public override Task OnKaiaStoreRefresh()
        {
            return Task.CompletedTask;
        }
    }
}
