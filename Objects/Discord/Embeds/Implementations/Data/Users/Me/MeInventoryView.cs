using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeViews
{
    public class MeInventoryView : KaiaPathEmbedPaginated
    {
        public MeInventoryView(KaiaUser User, CommandContext Context, int InventoryChunkSize) : base(new(),
                                                          Context,
                                                          0,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username)
        {
            this.User = User;
            this.InventoryChunkSize = InventoryChunkSize;
            ItemSelected += ItemSelectedAsync;
        }

        public static IEnumerable<KeyValuePair<KaiaInventoryItem, int>> GetItemsAndCounts(KaiaUser User)
        {
            List<KeyValuePair<KaiaInventoryItem, int>> ItemsAndTheirCounts = new();
            foreach (KaiaInventoryItem Item in User.Settings.Inventory.Items)
            {
                if (!ItemsAndTheirCounts.Exists(M => M.Key.DisplayName == Item.DisplayName))
                {
                    ItemsAndTheirCounts.Add(new(Item, 1));
                }
                else
                {
                    int Index = ItemsAndTheirCounts.FindIndex(M => M.Key.DisplayName == Item.DisplayName);
                    ItemsAndTheirCounts[Index] = new(ItemsAndTheirCounts[Index].Key, ItemsAndTheirCounts[Index].Value + 1);
                }
            }
            return ItemsAndTheirCounts;
        }

        public KaiaUser User { get; private set; }

        public int InventoryChunkSize { get; }

        private async void ItemSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            await Component.DeferAsync();
            KaiaInventoryItem? Listing = (await User.Settings.Inventory.GetItemsOfDisplayName(ItemsSelected.FirstOrDefault() ?? "")).FirstOrDefault();
            if (Listing != null)
            {
                InventoryItemView V = new(this, Listing, Context, User);
                await V.StartAsync(new(Component.User.Id));
                Dispose();
            }
        }

        protected override Task ClientRefreshAsync()
        {
            User = new(Context.UserContext.User.Id);

            MeView LandingPage = new(Context.UserContext.User.Username, User);
            LandingPage.WithField($"{Emotes.Counting.Inventory} inventory", $"`{User.Settings.Inventory.Items.Count}` {(User.Settings.Inventory.Items.Count == 1 ? "item" : "items")}");
            EmbedsAndOptions.Add(LandingPage, null);

            foreach (IEnumerable<KeyValuePair<KaiaInventoryItem, int>> ItemCountChunk in GetItemsAndCounts(User).Chunk(InventoryChunkSize))
            {
                ItemsPaginatedPage Embed = new(Context, ItemCountChunk);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KeyValuePair<KaiaInventoryItem, int> ItemChunk in ItemCountChunk)
                {
                    B.Add(new($"[{Strings.Economy.CurrencyEmote} {ItemChunk.Key.MarketCost}] {ItemChunk.Key.DisplayName}", ItemChunk.Key.DisplayName, ItemChunk.Key.Description, ItemChunk.Key.DisplayEmote));
                }
                EmbedsAndOptions.Add(Embed, B);
            }
            return Task.CompletedTask;
        }
    }
}
