using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    public class MeInventoryView : KaiaPathEmbedPaginated
    {
        public MeInventoryView(KaiaUser User, CommandContext Context, int InventoryChunkSize) : base(new(),
                                                          new(Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username),
                                                          Context,
                                                          0,
                                                          Emotes.Embeds.Back,
                                                          Emotes.Embeds.Forward,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username)
        {
            MeView LandingPage = new(Context.UserContext.User.Username, User);
            IEnumerable<KaiaInventoryItem[]> InventoryChunked = User.Settings.Inventory.Items.Chunk(InventoryChunkSize);
            LandingPage.WriteField($"{Emotes.Counting.Inventory} inventory", $"`{User.Settings.Inventory.Items.Count}` {(User.Settings.Inventory.Items.Count == 1 ? "item" : "items")}");
            this.EmbedsAndOptions.Add(LandingPage, null);
            List<KeyValuePair<KaiaInventoryItem, int>> ItemsAndTheirCounts = new();
            foreach (KaiaInventoryItem[] Chunk in InventoryChunked)
            {
                foreach (KaiaInventoryItem Item in Chunk)
                {
                    if (!ItemsAndTheirCounts.Exists(M => M.Key.DisplayName == M.Key.DisplayName))
                    {
                        ItemsAndTheirCounts.Add(new(Item, 1));
                    }
                    else
                    {
                        int Index = ItemsAndTheirCounts.FindIndex(M => M.Key.DisplayName == M.Key.DisplayName);
                        ItemsAndTheirCounts[Index] = new(ItemsAndTheirCounts[Index].Key, ItemsAndTheirCounts[Index].Value + 1);
                    }
                }
            }
            foreach (KeyValuePair<KaiaInventoryItem, int> ItemCount in ItemsAndTheirCounts)
            {
                List<string> Display = new();
                KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username);
                Display.Add($"[{ItemCount.Key.DisplayEmote}] {ItemCount.Key.DisplayName} [x{ItemCount.Value}]");
                Embed.WriteListToOneField("inventory", Display, "\n");
                this.EmbedsAndOptions.Add(Embed, null);
            }
        }
    }
}
