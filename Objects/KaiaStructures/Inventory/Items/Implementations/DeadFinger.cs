using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
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
            OnInteract = new(Strings.ItemStrings.DeadFinger.Message, new(0, KaiaSpellsRoom.Spells[new Random().Next(0, KaiaSpellsRoom.Spells.Count)]));
        }

        public override Task OnKaiaStoreRefresh()
        {
            OnInteract = new(Strings.ItemStrings.DeadFinger.Message, new(0, KaiaSpellsRoom.Spells[new Random().Next(0, KaiaSpellsRoom.Spells.Count)]));
            return Task.CompletedTask;
        }
    }
}
