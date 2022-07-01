namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class DeadFinger : KaiaInventoryItem
    {
        public DeadFinger() : base(DisplayName: Strings.ItemStrings.DeadFinger.Name,
                             Description: "a humanoid finger.",
                             MarketCost: 250.19,
                             CanInteractWithDirectly: true,
                             KaiaDisplaysThisOnTheStore: false,
                             UsersCanSellThis: true,
                             DisplayEmoteName: Emotes.Items.DeadFinger)
        {
            this.OnInteract = new(Strings.ItemStrings.DeadFinger.Message, new());
        }

        public override Task OnKaiaStoreRefresh()
        {
            this.OnInteract = new(Strings.ItemStrings.DeadFinger.Message, new(new Random().Next(1, 10)));
            return Task.CompletedTask;
        }
    }
}
