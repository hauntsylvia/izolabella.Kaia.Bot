using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
{
    public class BookRawView : KaiaPathEmbedRefreshable
    {
        public BookRawView(KaiaBook? Book, KaiaUser U) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library, Book?.Title ?? Strings.EmbedStrings.FakePaths.NotFound)
        {
            this.Book = Book;
            this.U = U;
        }

        public KaiaBook? Book { get; }

        public KaiaUser U { get; }

        public override Task RefreshAsync()
        {
            if (this.Book != null)
            {
                this.WithField($"author", $"`{this.Book.Author}`");
                this.WithField($"title", $"`{this.Book.Title}`");
                this.WithField($"current page", $"`{this.Book.CurrentPageIndex}` / `{this.Book.Pages}`");
                this.WithField($"current earnings", $"{Strings.Economy.CurrencyEmote} `{this.Book.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                if (this.Book.CurrentPageIndex < this.Book.Pages)
                {
                    this.WithField($"cost to read next page", $"{Strings.Economy.CurrencyEmote} `{this.Book.NextPageTurnCost}`");
                }
                this.WithField($"your balance", $"{Strings.Economy.CurrencyEmote} `{this.U.Settings.Inventory.Petals}`");
            }
            return Task.CompletedTask;
        }
    }
}
