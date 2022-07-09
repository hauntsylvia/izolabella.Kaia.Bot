using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemsPaginated : KaiaPathEmbedPaginated
    {
        public ItemsPaginated(CommandContext Context, IUser? FilterBy = null, bool IncludeUserListings = false, int ChunkSize = 4) : base(new(),
                                                                                                         Context,
                                                                                                         0,
                                                                                                         Strings.EmbedStrings.FakePaths.Global,
                                                                                                         Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            this.FilterBy = FilterBy;
            this.IncludeUserListings = IncludeUserListings;
            this.ChunkSize = ChunkSize;
            this.ItemSelected += this.StoreItemSelectedAsync;
        }

        public List<SaleListing> Listings { get; private set; } = new();

        public IUser? FilterBy { get; }

        public bool IncludeUserListings { get; }

        public int ChunkSize { get; }

        protected override async Task ClientRefreshAsync()
        {
            this.Listings = new();
            List<KaiaInventoryItem> AllItems = BaseImplementationUtil.GetItems<KaiaInventoryItem>()
                                                                  .Where(Item => Item.KaiaDisplaysThisOnTheStore)
                                                                  .ToList();

            if (this.FilterBy == null || this.FilterBy.Id == this.Context.Reference.CurrentUser.Id)
            {
                foreach (KaiaInventoryItem Item in AllItems)
                {
                    SaleListing L = new(new() { Item }, null, Item.MarketCost);
                    await L.StartSellingAsync();
                    this.Listings.Add(L);
                }
            }

            if (this.IncludeUserListings)
            {
                List<SaleListing> CurrentUserListings = await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>();
                this.Listings.AddRange(CurrentUserListings.Where(L => this.FilterBy == null || L.ListerId == this.FilterBy.Id));
            }

            IEnumerable<SaleListing[]> ListingsChunked = this.Listings.Where(A => A.IsListed).OrderBy(K => K.CostPerItem).OrderBy(K => K.Items.First().DisplayName).OrderBy(K => K.ListerId).Chunk(this.ChunkSize);

            foreach (SaleListing[] ListingsChunk in ListingsChunked)
            {
                ItemsPaginatedPage Embed = new(this.Context, ListingsChunk);
                List<SelectMenuOptionBuilder> B = new();
                foreach (SaleListing Listing in ListingsChunk)
                {
                    KaiaInventoryItem Item = Listing.Items.First();
                    string Description = $"[Sold By {(Listing.Lister != null ? this.Context.Reference.GetUser(Listing.Lister.Id)?.Username : "Kaia")}] {Item.Description}";
                    B.Add(new($"{Item.DisplayName}", Listing.Id.ToString(CultureInfo.InvariantCulture), Description.Length > 100 ? Description[..97] + "..." : Description, Item.DisplayEmote, false));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }
        }

        private async void StoreItemSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            await Component.DeferAsync();
            SaleListing? Listing = this.Listings.FirstOrDefault(Listing => Listing.Id.ToString(CultureInfo.InvariantCulture) == (ItemsSelected.FirstOrDefault() ?? ""));
            if (Listing != null)
            {
                ItemView V = new(this, this.Context, Listing, Emotes.Counting.BuyItem, Emotes.Counting.InteractItem, Emotes.Counting.SellItem, true);
                await V.StartAsync(new(Component.User.Id));
                this.Dispose();
            }
        }
    }
}
