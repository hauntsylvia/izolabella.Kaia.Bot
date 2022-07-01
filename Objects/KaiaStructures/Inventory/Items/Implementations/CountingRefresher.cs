namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class CountingRefresher : KaiaInventoryItem
    {
        public CountingRefresher() : base(DisplayName: Strings.ItemStrings.CountingRefresher.Name,
                                          Description: "allows u to keep counting even if u fail.",
                                          MarketCost: 124.98,
                                          CanInteractWithDirectly: false,
                                          KaiaDisplaysThisOnTheStore: true,
                                          UsersCanSellThis: false,
                                          DisplayEmoteName: Emotes.Items.CountingRefresher)
        {
        }
    }
}
