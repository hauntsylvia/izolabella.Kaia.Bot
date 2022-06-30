using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
{
    public class LibraryPage : KaiaPathEmbedRefreshable
    {
        public LibraryPage(IEnumerable<KaiaBook> BookChunkToWrite, KaiaUser U, bool IsFirstPage = false) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library)
        {
            this.BookChunk = BookChunkToWrite;
            this.U = U;
            this.IsFirstPage = IsFirstPage;
        }

        public IEnumerable<KaiaBook> BookChunk { get; }

        public KaiaUser U { get; }

        public bool IsFirstPage { get; }

        public override async Task ClientRefreshAsync()
        {
            if(this.IsFirstPage)
            {
                this.WithField("current total earnings", $"{Strings.Economy.CurrencyEmote} `{Math.Round((await this.U.LibraryProcessor.GetUserBooksAsync()).Sum(B => B.CurrentEarning), 2)}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
            }
            foreach (KaiaBook Book in this.BookChunk)
            {
                if (Book.AvailableUntil >= DateTime.UtcNow || Book.IsFinished)
                {
                    List<string> Display = new()
                    {
                        !Book.IsFinished ?
                            $"{Strings.Economy.CurrencyEmote} `{Book.NextPageTurnCost}` {(Book.CurrentPageIndex == 0 ? $"to begin reading" : $"to read the next page")}" :
                            $"u have finished this book"
                    };
                    if (!Book.IsFinished)
                    {
                        Display.Add($"`{Math.Round((Book.AvailableUntil - DateTime.UtcNow).TotalDays, 0)}` days left");
                    }
                    if (Book.CurrentPageIndex > 0)
                    {
                        Display.Add(!Book.IsFinished ? $"on page `{Book.CurrentPageIndex}` / `{Book.Pages}`" : $"`{Book.Pages}` total pages");
                        Display.Add($"currently earning - {Strings.Economy.CurrencyEmote} `{Book.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                    }
                    else
                    {
                        Display.Add($"if u begin reading - {Strings.Economy.CurrencyEmote} `{Book.NextPageEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                    }
                    this.WithListWrittenToField(Book.Title, Display, "\n");
                }
            }
        }
    }
}
