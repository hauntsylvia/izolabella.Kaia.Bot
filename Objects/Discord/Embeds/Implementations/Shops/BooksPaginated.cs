using Kaia.Bot.Objects.Constants.Embeds;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class BooksPaginated : KaiaPathEmbedPaginated
    {
        public BooksPaginated(CommandContext Context, LibraryViewFilters Filter) : base(new(),
                                                          EmbedDefaults.DefaultEmbedForNoItemsPresent,
                                                          Context,
                                                          0,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Library)
        {
            KaiaUser User = new(Context.UserContext.User.Id);
            IEnumerable<KaiaBook> KaiasBooks = KaiaLibrary.Books;
            List<KaiaBook> UserBooks = User.Settings.LibraryProcessor.GetUserBooksAsync().Result;
            List<KaiaBook> Inventory = KaiasBooks.Where(B => UserBooks.All(K => K.BookId != B.BookId)).ToList();
            Inventory.AddRange(UserBooks);
            bool SetBal = false;
            foreach (KaiaBook[] Chunk in Inventory.Where(IB =>
            {
                return Filter == LibraryViewFilters.Complete ? IB.IsFinished :
                       Filter == LibraryViewFilters.Incomplete ? !IB.IsFinished :
                       Filter == LibraryViewFilters.All;
            }).OrderBy(IB => IB.AvailableUntil).Chunk(2))
            {
                KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library);
                if (!SetBal)
                {
                    SetBal = true;
                    Embed.WithField("current total earnings", $"{Strings.Economy.CurrencyEmote} `{Math.Round(Inventory.Sum(B => B.CurrentEarning), 2)}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                }
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaBook Item in Chunk)
                {
                    if (Item.AvailableUntil >= DateTime.UtcNow || Item.IsFinished)
                    {
                        List<string> Display = new()
                        {
                            !Item.IsFinished ?
                                $"{Strings.Economy.CurrencyEmote} `{Item.NextPageTurnCost}` to read the next page" :
                                $"u have finished this book"
                        };
                        if (!Item.IsFinished)
                        {
                            Display.Add($"`{Math.Round((Item.AvailableUntil - DateTime.UtcNow).TotalDays, 0)}` days left");
                        }
                        if (Item.CurrentPageIndex > 0)
                        {
                            Display.Add(!Item.IsFinished ? $"on page `{Item.CurrentPageIndex}` / `{Item.Pages}`" : $"`{Item.Pages}` total pages");
                            Display.Add($"currently earning - {Strings.Economy.CurrencyEmote} `{Item.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                        }
                        else
                        {
                            Display.Add($"if u begin reading - {Strings.Economy.CurrencyEmote} `{Item.NextPageEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                        }
                        Embed.WithListWrittenToField(Item.Title, Display, "\n");
                        B.Add(new(Item.Title, Item.BookId, $"by {Item.Author}", Emotes.Counting.Book, false));
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
