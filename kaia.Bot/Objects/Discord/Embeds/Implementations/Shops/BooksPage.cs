﻿using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class BooksPage : CCBPathPaginatedEmbed
    {
        public BooksPage(CommandContext Context, int BookChunkSize) : base(new(),
                                                          new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Library),
                                                          Context,
                                                          0,
                                                          Emotes.Embeds.Back,
                                                          Emotes.Embeds.Forward,
                                                          Strings.EmbedStrings.PathIfNoGuild,
                                                          Strings.EmbedStrings.FakePaths.Library)
        {
            KaiaUser User = new(Context.UserContext.User.Id);
            IEnumerable<KaiaBook[]> InventoryChunked = KaiaLibrary.Books.Chunk(BookChunkSize);
            List<KaiaBook> UserBooks = User.Settings.LibraryProcessor.GetUserBooksAsync().Result;
            foreach (KaiaBook[] Chunk in InventoryChunked)
            {
                CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Library);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaBook Item in Chunk)
                {
                    KaiaBook PersonalBook = UserBooks.FirstOrDefault(PB => PB.BookId == Item.BookId) ?? Item;
                    if(PersonalBook.AvailableUntil >= DateTime.UtcNow)
                    {
                        List<string> Display = new()
                            {
                                $"{Strings.Economy.CurrencyEmote} `{PersonalBook.NextPageTurnCost}` to read page `{PersonalBook.CurrentPageIndex + 1}`",
                            };
                        if(PersonalBook.CurrentPageIndex > 0) 
                        {
                            Display.Add($"on page `{PersonalBook.CurrentPageIndex}` / `{PersonalBook.Pages}`");
                            Display.Add($"currently earning - {Strings.Economy.CurrencyEmote} `{PersonalBook.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                        }
                        else
                        {
                            Display.Add($"if u begin reading - {Strings.Economy.CurrencyEmote} `{PersonalBook.NextPageEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                        }
                        Embed.WriteListToOneField(PersonalBook.Title + $" by {PersonalBook.Author} [until `{PersonalBook.AvailableUntil.ToShortDateString()}`]", Display, "\n");
                        B.Add(new(PersonalBook.Title, PersonalBook.BookId, $"by {PersonalBook.Author}", Emotes.Counting.Book, false));
                    }
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            this.ItemSelected += this.ItemSelectedAsync;
        }

        private async void ItemSelectedAsync(CCBPathEmbed Page, int ZeroBasedIndex, global::Discord.WebSocket.SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            await Component.DeferAsync();
            await new BookView(this.Context, ItemsSelected.FirstOrDefault() ?? "", Emotes.Counting.Book).StartAsync(new(Component.User.Id));
            this.Dispose();
            this.ItemSelected -= this.ItemSelectedAsync;
        }
    }
}
