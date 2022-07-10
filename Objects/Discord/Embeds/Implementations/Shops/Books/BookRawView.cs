using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
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

        protected override Task ClientRefreshAsync()
        {
            if (Book != null)
            {
                WithField($"author", $"`{Book.Author}`");
                WithField($"title", $"`{Book.Title}`");
                WithField($"current page", $"`{Book.CurrentPageIndex}` / `{Book.Pages}`");
                WithField($"current earnings", $"{Strings.Economy.CurrencyEmote} `{Book.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                if (Book.CurrentPageIndex < Book.Pages)
                {
                    WithField($"cost to read next page", $"{Strings.Economy.CurrencyEmote} `{Book.NextPageTurnCost}`");
                }
                WithField($"your balance", $"{Strings.Economy.CurrencyEmote} `{U.Settings.Inventory.Petals}`");
            }
            return Task.CompletedTask;
        }
    }
}
