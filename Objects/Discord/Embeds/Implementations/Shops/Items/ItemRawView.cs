using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemRawView : KaiaPathEmbedRefreshable
    {
        public ItemRawView(CommandContext Context, KaiaInventoryItem ItemA, KaiaUser U, SaleListing Listing, bool DisplayBalancesMayGoUpMessage) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop, ItemA?.DisplayName ?? "none")
        {
            this.Context = Context;
            this.Item = ItemA ?? throw new NullReferenceException("A");
            this.U = U;
            this.Listing = Listing;
            this.DisplayBalancesMayGoUpMessage = DisplayBalancesMayGoUpMessage;
        }

        public CommandContext Context { get; }

        public KaiaInventoryItem Item { get; }

        public KaiaUser U { get; }

        public SaleListing Listing { get; }

        public bool DisplayBalancesMayGoUpMessage { get; }

        public override async Task ClientRefreshAsync()
        {
            bool IsKaiaListing = !(this.Listing.ListerId != null && this.Listing.ListerId != this.Context.Reference.Client.CurrentUser.Id);
            this.WithField($"[{Strings.Economy.CurrencyEmote} `{this.Listing.CostPerItem}`] {this.Item.DisplayName} {this.Item.DisplayEmote}", this.Item.Description);
            this.WithField("your balance", $"{Strings.Economy.CurrencyEmote} `{this.U.Settings.Inventory.Petals}`{(this.DisplayBalancesMayGoUpMessage ? "- balances may go up due to passive income from books every time it refreshes." : "")}");
            this.WithField($"number of {this.Item.DisplayName}s owned", $"`{(await this.U.Settings.Inventory.GetItemsOfDisplayName(this.Item)).Count()}`");
            if (!IsKaiaListing)
            {
                this.WithField($"number of {this.Item.DisplayName}s left in this listing", $"`{this.Listing.Items.Count}`");
            }
        }
    }
}
