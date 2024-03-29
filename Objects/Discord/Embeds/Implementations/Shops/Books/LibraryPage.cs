﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
{
    public class LibraryPage(IEnumerable<KaiaBook> BookChunkToWrite, KaiaUser U, bool IsFirstPage = false) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library)
    {
        public IEnumerable<KaiaBook> BookChunk { get; } = BookChunkToWrite;

        public KaiaUser U { get; } = U;

        public bool IsFirstPage { get; } = IsFirstPage;

        protected override async Task ClientRefreshAsync()
        {
            if (this.IsFirstPage)
            {
                this.WithField("current total earnings", $"{Strings.Economy.CurrencyEmote} `{Math.Round((await this.U.LibraryProcessor.GetUserBooksAsync()).Sum(B => B.CurrentEarning), 2)}` / `{TimeSpans.BookTickRate.TotalMinutes}` min. " +
                    $"{(this.U.TotalMultiplierOnBooks != 1 ? $"[* `{this.U.TotalMultiplierOnBooks}`]" : "")}");
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