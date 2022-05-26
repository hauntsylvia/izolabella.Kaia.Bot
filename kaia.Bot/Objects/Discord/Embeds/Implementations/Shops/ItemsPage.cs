﻿using Discord;
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
    public class ItemsPage : CCBPathPaginatedEmbed
    {
        public ItemsPage(CommandContext Context, List<ICCBInventoryItem> AllItems, int ChunkSize) : base(new(),
                                                                                                         new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop),
                                                                                                         Context,
                                                                                                         0,
                                                                                                         Emotes.Embeds.Back,
                                                                                                         Emotes.Embeds.Forward,
                                                                                                         Strings.EmbedStrings.PathIfNoGuild,
                                                                                                         Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            IEnumerable<ICCBInventoryItem[]> ItemsChunked = AllItems.Chunk(ChunkSize);

            foreach (ICCBInventoryItem[] Items in ItemsChunked)
            {
                CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop);
                List<SelectMenuOptionBuilder> B = new();
                foreach (ICCBInventoryItem Item in Items)
                {
                    Embed.WriteField($"[{Strings.Economy.CurrencyEmote} `{Item.Cost}`] {Item.DisplayName}  {Item.DisplayEmote}", Item.Description);
                    B.Add(new($"{Item.DisplayName}", Item.DisplayName, Item.Description, Item.DisplayEmote, false));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            this.ItemSelected += this.StoreItemSelectedAsync;
        }

        private async void StoreItemSelectedAsync(CCBPathEmbed Page, int ZeroBasedIndex, global::Discord.WebSocket.SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            ICCBInventoryItem? Item = InterfaceImplementationController.GetItems<ICCBInventoryItem>().FirstOrDefault(X => X.DisplayName == (ItemsSelected.FirstOrDefault() ?? ""));
            if(Item != null)
            {
                await Component.DeferAsync();
                await new ItemView(this.Context, Item, Emotes.Counting.BuyItem, Emotes.Counting.InteractItem).StartAsync(new(Component.User.Id));
                this.Dispose();
                this.ItemSelected -= this.StoreItemSelectedAsync;
            }
        }
    }
}