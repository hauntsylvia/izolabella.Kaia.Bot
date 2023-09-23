using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemRawView(CommandContext Context, KaiaInventoryItem ItemA, KaiaUser U, SaleListing Listing, bool DisplayBalancesMayGoUpMessage) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop, ItemA.DisplayName)
    {
        public CommandContext Context { get; } = Context;

        public KaiaInventoryItem Item { get; } = ItemA;

        public KaiaUser U { get; } = U;

        public SaleListing Listing { get; } = Listing;

        public bool DisplayBalancesMayGoUpMessage { get; } = DisplayBalancesMayGoUpMessage;

        protected override async Task ClientRefreshAsync()
        {
            bool IsKaiaListing = !(this.Listing.ListerId != null && this.Listing.ListerId != this.Context.Reference.CurrentUser.Id);
            this.WithField($"[{Strings.Economy.CurrencyEmote} `{this.Listing.CostPerItem}`] {this.Item.DisplayName} {this.Item.DisplayEmote}", this.Item.Description);
            this.WithField("your balance", $"{Strings.Economy.CurrencyEmote} `{this.U.Settings.Inventory.Petals}`{(this.DisplayBalancesMayGoUpMessage ? "- balances may go up due to passive income from books every time it refreshes." : "")}");
            this.WithField($"number of {this.Item.DisplayName}s owned", $"`{(await this.U.Settings.Inventory.GetItemsOfDisplayNameFromItem(this.Item)).Count()}`");
            if (!IsKaiaListing)
            {
                this.WithField($"number of {this.Item.DisplayName}s left in this listing", $"`{this.Listing.Items.Count}`");
            }
        }
    }
}