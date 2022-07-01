using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
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
            this.WithField($"[{Strings.Economy.CurrencyEmote} `{this.SingleListing.CostPerItem}`] {this.SingleListing.Items.First().DisplayName} {this.SingleListing.Items.First().DisplayEmote} [x{this.SingleListing.Items.Count}]", this.SingleListing.Items.First().Description);
            this.WithListWrittenToField("notices", new()
            {
                "**type a number in the channel this embed was sent in to set the price per item**",
                "items will be validated when submitted",
                "buying back items from ur own listing does not grant a refund"
            }, ",\n");
            if((await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>()).Any(S => S.ListerId == this.SingleListing.ListerId))
            {
                this.WithField("warning", "users may only submit up to one active listing at a time");
            }
        }
    }
}
