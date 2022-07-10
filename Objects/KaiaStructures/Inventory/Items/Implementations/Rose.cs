using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class Rose : KaiaInventoryItem
    {
        public Rose() : base(DisplayName: Strings.ItemStrings.Rose.Name,
                             Description: "a rose!",
                             MarketCost: 34.98,
                             CanInteractWithDirectly: true,
                             KaiaDisplaysThisOnTheStore: true,
                             UsersCanSellThis: true,
                             DisplayEmoteName: Emotes.Items.Rose)
        {
            double PetalsEarned = Math.Round(new Random().Next(-100, 200) + new Random().NextDouble(), 2);
            string Message = PetalsEarned < 0 ? Strings.ItemStrings.Rose.RoseStab : Strings.ItemStrings.Rose.RosePretty;
            OnInteract = new(Message, new(PetalsEarned));
        }

        public override Task OnKaiaStoreRefresh()
        {
            double PetalsEarned = Math.Round(new Random().Next(-100, 200) + new Random().NextDouble(), 2);
            string Message = PetalsEarned < 0 ? Strings.ItemStrings.Rose.RoseStab : Strings.ItemStrings.Rose.RosePretty;
            OnInteract = new(Message, new(PetalsEarned));
            return Task.CompletedTask;
        }
    }
}
