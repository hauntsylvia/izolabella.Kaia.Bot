﻿using Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
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
            if(this.Listings != null)
            {
                foreach (SaleListing Listing in this.Listings.Where(L => L.IsListed))
                {
                    KaiaInventoryItem Item = Listing.Items.First();

                    this.WithField($"[{Strings.Economy.CurrencyEmote} `{Listing.CostPerItem}`] {Item.DisplayName} {Item.DisplayEmote}", Item.Description);
                }
            }
            else if(this.ItemCountChunk != null)
            {
                this.ItemCountChunk = MeInventoryView.GetItemsAndCounts(this.Context, new(this.Context.UserContext.User.Id));
                List<string> Display = new();
                foreach (KeyValuePair<KaiaInventoryItem, int> ItemCount in this.ItemCountChunk)
                {
                    Display.Add($"[{ItemCount.Key.DisplayEmote}] {ItemCount.Key.DisplayName} [x{ItemCount.Value}]");
                }

                this.WithListWrittenToField("inventory", Display, "\n");
            }
            return Task.CompletedTask;
        }
    }
}
