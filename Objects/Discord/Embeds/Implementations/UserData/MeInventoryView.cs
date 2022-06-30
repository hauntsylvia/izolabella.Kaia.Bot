using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
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
            MeView LandingPage = new(Context.UserContext.User.Username, User);
            IEnumerable<KaiaInventoryItem[]> InventoryChunked = User.Settings.Inventory.Items.Chunk(InventoryChunkSize);
            LandingPage.WithField($"{Emotes.Counting.Inventory} inventory", $"`{User.Settings.Inventory.Items.Count}` {(User.Settings.Inventory.Items.Count == 1 ? "item" : "items")}");
            this.EmbedsAndOptions.Add(LandingPage, null);
            List<KeyValuePair<KaiaInventoryItem, int>> ItemsAndTheirCounts = new();
            foreach (KaiaInventoryItem[] Chunk in InventoryChunked)
            {
                foreach (KaiaInventoryItem Item in Chunk)
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
            }
            foreach (IEnumerable<KeyValuePair<KaiaInventoryItem, int>> ItemCountChunk in ItemsAndTheirCounts.Chunk(InventoryChunkSize))
            {
                KaiaPathEmbedRefreshable Embed = new ItemsPaginatedPage(Context, ItemCountChunk);
                this.EmbedsAndOptions.Add(Embed, null);
            }
        }
    }
}
