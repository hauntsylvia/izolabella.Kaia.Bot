using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeData
{
    public class InventoryItemViewRaw(KaiaInventoryItem Item) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Inventory, Item.DisplayName)
    {
        public KaiaInventoryItem Item { get; } = Item;

        protected override Task ClientRefreshAsync()
        {
            this.WithField(this.Item.DisplayString, this.Item.Description);
            if (this.Item.ReceivedAt.HasValue)
            {
                this.WithField("received at", $"`{this.Item.ReceivedAt.Value.ToShortDateString()}`");
            }
            this.WithField("unique identifier", $"`{this.Item.Id.ToString(CultureInfo.InvariantCulture)}`");
            return Task.CompletedTask;
        }
    }
}