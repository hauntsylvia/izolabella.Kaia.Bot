using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemCreateSaleListingRaw : KaiaPathEmbedRefreshable
    {
        public ItemCreateSaleListingRaw(SaleListing SingleListing) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            this.SingleListing = SingleListing;
        }

        public SaleListing SingleListing { get; }

        protected override async Task ClientRefreshAsync()
        {
            WithField($"[{Strings.Economy.CurrencyEmote} `{SingleListing.CostPerItem}`] {SingleListing.Items.First().DisplayName} {SingleListing.Items.First().DisplayEmote} [x{SingleListing.Items.Count}]", SingleListing.Items.First().Description);
            WithListWrittenToField("notices", new List<string>()
            {
                "**type a number in the channel this embed was sent in to set the price per item**",
                "items will be validated when submitted",
                "buying back items from ur own listing does not grant a refund"
            }, ",\n");
            if ((await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>()).Any(S => S.ListerId == SingleListing.ListerId))
            {
                WithField("warning", "users may only submit up to one active listing at a time");
            }
        }
    }
}
