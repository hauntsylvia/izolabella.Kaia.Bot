﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeViews;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemsPaginatedPage : KaiaPathEmbedRefreshable
    {
        public ItemsPaginatedPage(CommandContext Context, IEnumerable<SaleListing> Listings) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            this.Context = Context;
            this.Listings = Listings;
        }

        public ItemsPaginatedPage(CommandContext Context, IEnumerable<KeyValuePair<KaiaInventoryItem, int>> ItemCountChunk) : base(Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Inventory)
        {
            this.Context = Context;
            this.ItemCountChunk = ItemCountChunk;
        }

        public CommandContext Context { get; }

        public IEnumerable<KeyValuePair<KaiaInventoryItem, int>>? ItemCountChunk { get; private set; }

        public IEnumerable<SaleListing>? Listings { get; }

        protected override Task ClientRefreshAsync()
        {
            if (Listings != null)
            {
                foreach (SaleListing Listing in Listings.Where(L => L.IsListed))
                {
                    KaiaInventoryItem Item = Listing.Items.First();

                    WithField($"[{Strings.Economy.CurrencyEmote} `{Listing.CostPerItem}`] {Item.DisplayName} {Item.DisplayEmote}", Item.Description);
                }
            }
            else if (ItemCountChunk != null)
            {
                ItemCountChunk = MeInventoryView.GetItemsAndCounts(new(Context.UserContext.User.Id));
                List<string> Display = new();
                foreach (KeyValuePair<KaiaInventoryItem, int> ItemCount in ItemCountChunk)
                {
                    Display.Add($"[{ItemCount.Key.DisplayEmote}] {ItemCount.Key.DisplayName} [x{ItemCount.Value}]");
                }

                WithListWrittenToField("inventory", Display, "\n");
            }
            return Task.CompletedTask;
        }
    }
}
