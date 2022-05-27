using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class ItemsPage : KaiaPathEmbedPaginated
    {
        public ItemsPage(CommandContext Context, List<KaiaInventoryItem> AllItems, int ChunkSize) : base(new(),
                                                                                                         new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop),
                                                                                                         Context,
                                                                                                         0,
                                                                                                         Emotes.Embeds.Back,
                                                                                                         Emotes.Embeds.Forward,
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
                    Embed.WriteField($"[{Strings.Economy.CurrencyEmote} `{Item.Cost}`] {Item.DisplayName}  {Item.DisplayEmote}", Item.Description);
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
            KaiaInventoryItem? Item = InterfaceImplementationController.GetItems<KaiaInventoryItem>().FirstOrDefault(X => X.DisplayName == (ItemsSelected.FirstOrDefault() ?? ""));
            if (Item != null)
            {
                await Component.DeferAsync();
                ItemView V = new(this.Context, Item, Emotes.Counting.BuyItem, Emotes.Counting.InteractItem, true);
                await V.StartAsync(new(Component.User.Id));
                V.BackRequested += this.BackRequestedAsync;
                this.Dispose();
                this.ItemSelected -= this.StoreItemSelectedAsync;
            }
        }

        private async void BackRequestedAsync(global::Discord.WebSocket.SocketMessageComponent Component)
        {
            this.Dispose();
            await Component.DeferAsync();
            await new ItemsPage(this.Context, this.AllItems, this.ChunkSize).StartAsync();
        }
    }
}
