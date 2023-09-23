using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
{
    public class BookRawView(KaiaBook? Book, KaiaUser U) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library, Book?.Title ?? Strings.EmbedStrings.FakePaths.NotFound)
    {
        public KaiaBook? Book { get; } = Book;

        public KaiaUser U { get; } = U;

        protected override Task ClientRefreshAsync()
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