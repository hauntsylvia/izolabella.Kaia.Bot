﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemCreateSaleListingRaw(SaleListing SingleListing) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop)
    {
        public SaleListing SingleListing { get; } = SingleListing;

        protected override async Task ClientRefreshAsync()
        {
            this.WithField($"[{Strings.Economy.CurrencyEmote} `{this.SingleListing.CostPerItem}`] {this.SingleListing.Items.First().DisplayName} {this.SingleListing.Items.First().DisplayEmote} [x{this.SingleListing.Items.Count}]", this.SingleListing.Items.First().Description);
            this.WithListWrittenToField("notices", new List<string>()
        {
            "**type a number in the channel this embed was sent in to set the price per item**",
            "items will be validated when submitted",
            "buying back items from ur own listing does not grant a refund"
        }, ",\n");
            if ((await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>()).Any(S => S.ListerId == this.SingleListing.ListerId))
            {
                this.WithField("warning", "users may only submit up to one active listing at a time");
            }
        }
    }
}