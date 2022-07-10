using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemRawView : KaiaPathEmbedRefreshable
    {
        public ItemRawView(CommandContext Context, KaiaInventoryItem ItemA, KaiaUser U, SaleListing Listing, bool DisplayBalancesMayGoUpMessage) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop, ItemA.DisplayName)
        {
            this.Context = Context;
            Item = ItemA;
            this.U = U;
            this.Listing = Listing;
            this.DisplayBalancesMayGoUpMessage = DisplayBalancesMayGoUpMessage;
        }

        public CommandContext Context { get; }

        public KaiaInventoryItem Item { get; }

        public KaiaUser U { get; }

        public SaleListing Listing { get; }

        public bool DisplayBalancesMayGoUpMessage { get; }

        protected override async Task ClientRefreshAsync()
        {
            bool IsKaiaListing = !(Listing.ListerId != null && Listing.ListerId != Context.Reference.CurrentUser.Id);
            WithField($"[{Strings.Economy.CurrencyEmote} `{Listing.CostPerItem}`] {Item.DisplayName} {Item.DisplayEmote}", Item.Description);
            WithField("your balance", $"{Strings.Economy.CurrencyEmote} `{U.Settings.Inventory.Petals}`{(DisplayBalancesMayGoUpMessage ? "- balances may go up due to passive income from books every time it refreshes." : "")}");
            WithField($"number of {Item.DisplayName}s owned", $"`{(await U.Settings.Inventory.GetItemsOfDisplayNameFromItem(Item)).Count()}`");
            if (!IsKaiaListing)
            {
                WithField($"number of {Item.DisplayName}s left in this listing", $"`{Listing.Items.Count}`");
            }
        }
    }
}
