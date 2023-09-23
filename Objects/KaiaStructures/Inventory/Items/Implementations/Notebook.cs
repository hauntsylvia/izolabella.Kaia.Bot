using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class Notebook : KaiaInventoryItem
    {
        public Notebook() : base(DisplayName: Strings.ItemStrings.Notebook.Name,
                             Description: "a notebook.",
                             MarketCost: 1.48,
                             CanInteractWithDirectly: true,
                             KaiaDisplaysThisOnTheStore: false,
                             UsersCanSellThis: true,
                             DisplayEmoteName: Emotes.Items.Notebook)
        {
            this.OnInteract = new(Strings.ItemStrings.Notebook.GetMessage(), new(new Random().Next(1, 10)));
        }

        public override Task OnKaiaStoreRefresh()
        {
            this.OnInteract = new(Strings.ItemStrings.Notebook.GetMessage(), new(new Random().Next(1, 10)));
            return Task.CompletedTask;
        }
    }
}