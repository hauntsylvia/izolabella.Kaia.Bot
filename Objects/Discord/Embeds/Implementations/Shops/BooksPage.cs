using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class BooksPage : KaiaPathEmbedPaginated
    {
        public BooksPage(CommandContext Context, LibraryViewFilters Filter) : base(new(),
                                                          new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library),
                                                          Context,
                                                          0,
                                                          Emotes.Embeds.Back,
                                                          Emotes.Embeds.Forward,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Library)
        {
            KaiaUser User = new(Context.UserContext.User.Id);
            IEnumerable<KaiaBook> KaiasBooks = KaiaLibrary.Books;
            List<KaiaBook> UserBooks = User.Settings.LibraryProcessor.GetUserBooksAsync().Result;
            List<KaiaBook> Inventory = KaiasBooks.Where(B => UserBooks.All(K => K.BookId != B.BookId)).ToList();
            Inventory.AddRange(UserBooks);
            foreach (KaiaBook[] Chunk in Inventory.Where(IB => 
                                                         Filter == LibraryViewFilters.ShowFinished ? IB.IsFinished : 
                                                         Filter == LibraryViewFilters.ShowUnfinished ? !IB.IsFinished : 
                                                         Filter == LibraryViewFilters.ShowAll)
                                                         .OrderBy(IB => IB.AvailableUntil)
                                                         .Chunk(3))
            {
                KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaBook Item in Chunk)
                {
                    KaiaBook PersonalBook = UserBooks.FirstOrDefault(PB => PB.BookId == Item.BookId) ?? Item;
                    if (PersonalBook.AvailableUntil >= DateTime.UtcNow)
                    {
                        List<string> Display = new()
                        {
                            !PersonalBook.IsFinished ? 
                                $"{Strings.Economy.CurrencyEmote} `{PersonalBook.NextPageTurnCost}` to read the next page" :
                                $"u have finished this book",
                        };
                        if (PersonalBook.CurrentPageIndex > 0)
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

        private async void ItemSelectedAsync(KaiaPathEmbed Page, int ZeroBasedIndex, global::Discord.WebSocket.SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            await Component.DeferAsync();
            BookView V = new(this, this.Context, ItemsSelected.FirstOrDefault() ?? "", Emotes.Counting.Book, true);
            await V.StartAsync(new(Component.User.Id));
            this.Dispose();
        }
    }
}
