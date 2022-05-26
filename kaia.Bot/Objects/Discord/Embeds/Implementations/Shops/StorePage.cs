using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using Kaia.Bot.Objects.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class StorePage : CCBPathPaginatedEmbed
    {
        public StorePage(CommandContext Context, List<ICCBInventoryItem> AllItems, int ChunkSize) : base(new(),
                                                                                                         new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop),
                                                                                                         Context,
                                                                                                         0,
                                                                                                         Emotes.Embeds.Back,
                                                                                                         Emotes.Embeds.Forward,
                                                                                                         Strings.EmbedStrings.PathIfNoGuild,
                                                                                                         Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            Dictionary<CCBPathEmbed, List<SelectMenuOptionBuilder>> D = new();
            IEnumerable<ICCBInventoryItem[]> ItemsChunked = AllItems.Chunk(ChunkSize);

            foreach (ICCBInventoryItem[] Items in ItemsChunked)
            {
                CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop);
                List<SelectMenuOptionBuilder> B = new();
                foreach (ICCBInventoryItem Item in Items)
                {
                    Embed.WriteField($"[{Strings.Economy.CurrencyEmote} `{Item.Cost}`] {Item.DisplayName}  {Item.DisplayEmote}", Item.Description);
                    B.Add(new($"[{Strings.Economy.CurrencyEmote} `{Item.Cost}`] {Item.DisplayName}  {Item.DisplayEmote}", Item.DisplayName, Item.Description, Item.DisplayEmote, false));
                }
                D.Add(Embed, B);
            }
        }
    }
}
