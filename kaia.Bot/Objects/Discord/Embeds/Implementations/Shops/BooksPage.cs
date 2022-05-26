using Discord;
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
            CCBUser User = new(Context.UserContext.User.Id);
            IEnumerable<KaiaBook[]> InventoryChunked = KaiaLibrary.Books.Where(B => !User.Settings.LibraryProcessor.UserHasBookOfIdAsync(B.BookId).Result).Chunk(BookChunkSize);
            foreach (KaiaBook[] Chunk in InventoryChunked)
            {
                CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Library);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaBook Item in Chunk)
                {
                    if(Item.AvailableUntil >= DateTime.UtcNow)
                    {
                        Embed.WriteListToOneField(Item.Title + $" by {Item.Author}",
                            new()
                            {
                            $"`{Item.Pages}` pages",
                            $"no longer available after `{Item.AvailableUntil.ToShortDateString()}`",
                            $"{Strings.Economy.CurrencyEmote} `{Item.NextPageTurnCost}` to read",
                            $"{Strings.Economy.CurrencyEmote} `{Item.NextPageEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min."
                            }, "\n");
                        B.Add(new(Item.Title, Item.BookId, $"by {Item.Author}", Emotes.Counting.Book, false));
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
