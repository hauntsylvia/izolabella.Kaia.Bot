using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;
using izolabella.Util;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
{
    public class BookView : KaiaItemContentView
    {
        public BookView(BooksPaginated? From, CommandContext Context, string BookId, IEmote ReadPageEmote, bool CanGoBack) : base(From, Context, CanGoBack)
        {
            this.ReadNextPageId = $"readnextpage-lbv-{IdGenerator.CreateNewId()}";
            this.BookId = BookId;
            this.ReadPageEmote = ReadPageEmote;
        }

        public string ReadNextPageId { get; }
        public string BookId { get; }
        public IEmote ReadPageEmote { get; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.Client.ButtonExecuted -= this.ButtonExecutedAsync;
        }

        private async Task<KaiaBook?> GetUserBookAsync(KaiaUser U)
        {
            return (await U.LibraryProcessor.GetUserBooksAsync()).FirstOrDefault(B => B.BookId == this.BookId) ?? KaiaLibrary.GetActiveBookById(this.BookId) ?? null;
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            KaiaBook? Book = await this.GetUserBookAsync(U);
            ComponentBuilder CB = (await this.GetDefaultComponents()).WithButton("Read", this.ReadNextPageId, ButtonStyle.Secondary, this.ReadPageEmote, disabled: Book != null && Book.CurrentPageIndex >= Book.Pages);
            return CB;
        }

        public override async Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U)
        {
            KaiaBook? Book = await this.GetUserBookAsync(U);
            KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library, Book?.Title ?? Strings.EmbedStrings.FakePaths.NotFound);
            if (Book != null)
            {
                Embed.WithField($"author", $"`{Book.Author}`");
                Embed.WithField($"title", $"`{Book.Title}`");
                Embed.WithField($"current page", $"`{Book.CurrentPageIndex}` / `{Book.Pages}`");
                Embed.WithField($"current earnings", $"{Strings.Economy.CurrencyEmote} `{Book.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                if (Book.CurrentPageIndex < Book.Pages)
                {
                    Embed.WithField($"cost to read next page", $"{Strings.Economy.CurrencyEmote} `{Book.NextPageTurnCost}`");
                }
                Embed.WithField($"your balance", $"{Strings.Economy.CurrencyEmote} `{U.Settings.Inventory.Petals}`");
            }
            return Embed;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbed E = await this.GetEmbedAsync(U);
            ComponentBuilder Com = await this.GetComponentsAsync(U);
            _ = await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
            this.Context.Reference.Client.ButtonExecuted += this.ButtonExecutedAsync;
        }

        private async Task ButtonExecutedAsync(SocketMessageComponent Component)
        {
            if (Component.Data.CustomId == this.ReadNextPageId && Component.User.Id == this.Context.UserContext.User.Id)
            {
                KaiaUser U = new(Component.User.Id);
                KaiaBook? Book = await this.GetUserBookAsync(U);
                if (Book != null && U.Settings.Inventory.Petals >= Book.NextPageTurnCost)
                {
                    if (!await U.LibraryProcessor.UserHasBookOfIdAsync(this.BookId) && KaiaLibrary.GetActiveBookById(this.BookId) is KaiaBook KBook)
                    {
                        await U.LibraryProcessor.AddBookAsync(KBook);
                    }
                    U.Settings.Inventory.Petals -= Book.NextPageTurnCost;
                    await U.LibraryProcessor.IncrementBookAsync(Book.BookId);
                    await U.SaveAsync();
                }
                KaiaPathEmbed E = await this.GetEmbedAsync(U);
                ComponentBuilder Com = await this.GetComponentsAsync(U);
                await Component.UpdateAsync(C =>
                {
                    C.Embed = E.Build();
                    C.Components = Com.Build();
                });
            }
        }
    }
}
