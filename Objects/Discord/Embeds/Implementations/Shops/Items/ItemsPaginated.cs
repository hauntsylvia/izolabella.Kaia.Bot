using izolabella.Util;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemsPaginated : KaiaPathEmbedPaginated
    {
        public ItemsPaginated(CommandContext Context, IUser? FilterBy = null, bool IncludeUserListings = false, int ChunkSize = 4) : base(new(),
                                                                                                         Context,
                                                                                                         0,
                                                                                                         Strings.EmbedStrings.FakePaths.Global,
                                                                                                         Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            List<KaiaInventoryItem> AllItems = BaseImplementationUtil.GetItems<KaiaInventoryItem>()
                                                                  .Where(Item => Item.KaiaDisplaysThisOnTheStore)
                                                                  .ToList();
            List<SaleListing> Listings = new();
            if(FilterBy == null || FilterBy.Id == Context.Reference.Client.CurrentUser.Id)
            {
                foreach (KaiaInventoryItem Item in AllItems)
                {
                    SaleListing L = new(new() { Item }, null, Item.MarketCost);
                    L.StartSellingAsync().Wait();
                    Listings.Add(L);
                }
            }

            if(IncludeUserListings)
            {
                List<SaleListing> CurrentUserListings = DataStores.SaleListingsStore.ReadAllAsync<SaleListing>().Result;
                Listings.AddRange(CurrentUserListings.Where(L => FilterBy == null || L.ListerId == FilterBy.Id));
            }

            IEnumerable<SaleListing[]> ListingsChunked = Listings.Where(A => A.IsListed).OrderBy(K => K.CostPerItem).OrderBy(K => K.Items.First().DisplayName).OrderBy(K => K.ListerId).Chunk(ChunkSize);

            foreach (SaleListing[] ListingsChunk in ListingsChunked)
            {
                ItemsPaginatedPage Embed = new(this.Context, ListingsChunk);
                List<SelectMenuOptionBuilder> B = new();
                foreach (SaleListing Listing in ListingsChunk)
                {
                    KaiaInventoryItem Item = Listing.Items.First();
                    string Description = $"[Sold By {(Listing.Lister != null ? Context.Reference.Client.GetUser(Listing.Lister.Id).Username : "Kaia")}] {Item.Description}";
                    B.Add(new($"{Item.DisplayName}", Listing.Id.ToString(CultureInfo.InvariantCulture), Description.Length > 100 ? Description[..97] + "..." : Description, Item.DisplayEmote, false));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            this.ItemSelected += this.StoreItemSelectedAsync;
            this.AllListings = Listings;
            this.FilterBy = FilterBy;
            this.IncludeUserListings = IncludeUserListings;
            this.ChunkSize = ChunkSize;
        }

        public List<SaleListing> AllListings { get; }

        public IUser? FilterBy { get; }

        public bool IncludeUserListings { get; }

        public int ChunkSize { get; }

        private async void StoreItemSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            await Component.DeferAsync();
            SaleListing? Listing = this.AllListings.FirstOrDefault(Listing => Listing.Id.ToString(CultureInfo.InvariantCulture) == (ItemsSelected.FirstOrDefault() ?? ""));
            if (Listing != null)
            {
                ItemView V = new(this, this.Context, Listing, Emotes.Counting.BuyItem, Emotes.Counting.InteractItem, Emotes.Counting.SellItem, true);
                await V.StartAsync(new(Component.User.Id));
                this.Dispose();
            }
        }
    }
}
