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

        public override Task ClientRefreshAsync()
        {
            this.WithField($"[{Strings.Economy.CurrencyEmote} `{this.SingleListing.CostPerItem}`] {this.SingleListing.Items.First().DisplayName} {this.SingleListing.Items.First().DisplayEmote} [x{this.SingleListing.Items.Count}]", this.SingleListing.Items.First().Description);
            return Task.CompletedTask;
        }
    }
}
