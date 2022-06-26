using Kaia.Bot.Objects.Constants.Embeds;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class ItemsPaginated : KaiaPathEmbedPaginated
    {
        public ItemsPaginated(CommandContext Context, List<KaiaInventoryItem> AllItems, int ChunkSize) : base(new(),
                                                                                                         EmbedDefaults.DefaultEmbedForNoItemsPresent,
                                                                                                         Context,
                                                                                                         0,
                                                                                                         Strings.EmbedStrings.FakePaths.Global,
                                                                                                         Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            IEnumerable<KaiaInventoryItem[]> ItemsChunked = AllItems.Chunk(ChunkSize);

            foreach (KaiaInventoryItem[] Items in ItemsChunked)
            {
                KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaInventoryItem Item in Items)
                {
                    Embed.WithField($"[{Strings.Economy.CurrencyEmote} `{Item.Cost}`] {Item.DisplayName}  {Item.DisplayEmote}", Item.Description);
                    B.Add(new($"{Item.DisplayName}", Item.DisplayName, Item.Description, Item.DisplayEmote, false));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            this.ItemSelected += this.StoreItemSelectedAsync;
            this.AllItems = AllItems;
            this.ChunkSize = ChunkSize;
        }
        public List<KaiaInventoryItem> AllItems { get; }
        public int ChunkSize { get; }

        private async void StoreItemSelectedAsync(KaiaPathEmbed Page, int ZeroBasedIndex, global::Discord.WebSocket.SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            KaiaInventoryItem? Item = BaseImplementationUtil.GetItems<KaiaInventoryItem>().FirstOrDefault(X => X.DisplayName == (ItemsSelected.FirstOrDefault() ?? ""));
            if (Item != null)
            {
                await Component.DeferAsync();
                ItemView V = new(this, this.Context, Item, Emotes.Counting.BuyItem, Emotes.Counting.InteractItem, true);
                await V.StartAsync(new(Component.User.Id));
                this.Dispose();
            }
        }
    }
}
