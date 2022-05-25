using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.User_Data
{
    internal class MeInventory : CCBPathPaginatedEmbed
    {
        public MeInventory(CCBUser User, CommandContext Context, int InventoryChunkSize) : base(new(),
                                                          Context,
                                                          0,
                                                          Emotes.Embeds.Back,
                                                          Emotes.Embeds.Forward,
                                                          Strings.EmbedStrings.PathIfNoGuild,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username)
        {
            this.Embeds.Add(new MeView(Context.UserContext.User.Username, User));
            IEnumerable<ICCBInventoryItem[]> InventoryChunked = User.Settings.Inventory.Items.Chunk(InventoryChunkSize);
            List<KeyValuePair<ICCBInventoryItem, int>> ItemsAndTheirCounts = new();
            foreach (ICCBInventoryItem[] Chunk in InventoryChunked)
            {
                foreach (ICCBInventoryItem Item in Chunk)
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
            foreach (KeyValuePair<ICCBInventoryItem, int> ItemCount in ItemsAndTheirCounts)
            {
                List<string> Display = new();
                CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username);
                Display.Add($"[{ItemCount.Key.DisplayEmote}] {ItemCount.Key.DisplayName} [x{ItemCount.Value}]");
                Embed.WriteListToOneField("inventory", Display, "\n");
                this.Embeds.Add(Embed);
            }
        }
    }
}
