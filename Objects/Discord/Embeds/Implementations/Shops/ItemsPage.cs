using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class ItemsPage : KaiaPathEmbedPaginated
    {
        public ItemsPage(CommandContext Context, List<IKaiaInventoryItem> AllItems, int ChunkSize) : base(new(),
                                                                                                         new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop),
                                                                                                         Context,
                                                                                                         0,
                                                                                                         Emotes.Embeds.Back,
                                                                                                         Emotes.Embeds.Forward,
                                                                                                         Strings.EmbedStrings.PathIfNoGuild,
                                                                                                         Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            IEnumerable<IKaiaInventoryItem[]> ItemsChunked = AllItems.Chunk(ChunkSize);

            foreach (IKaiaInventoryItem[] Items in ItemsChunked)
            {
                KaiaPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop);
                List<SelectMenuOptionBuilder> B = new();
                foreach (IKaiaInventoryItem Item in Items)
                {
                    Embed.WriteField($"[{Strings.Economy.CurrencyEmote} `{Item.Cost}`] {Item.DisplayName}  {Item.DisplayEmote}", Item.Description);
                    B.Add(new($"{Item.DisplayName}", Item.DisplayName, Item.Description, Item.DisplayEmote, false));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            this.ItemSelected += this.StoreItemSelectedAsync;
        }

        private async void StoreItemSelectedAsync(KaiaPathEmbed Page, int ZeroBasedIndex, global::Discord.WebSocket.SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            IKaiaInventoryItem? Item = InterfaceImplementationController.GetItems<IKaiaInventoryItem>().FirstOrDefault(X => X.DisplayName == (ItemsSelected.FirstOrDefault() ?? ""));
            if (Item != null)
            {
                await Component.DeferAsync();
                await new ItemView(this.Context, Item, Emotes.Counting.BuyItem, Emotes.Counting.InteractItem).StartAsync(new(Component.User.Id));
                this.Dispose();
                this.ItemSelected -= this.StoreItemSelectedAsync;
            }
        }
    }
}
