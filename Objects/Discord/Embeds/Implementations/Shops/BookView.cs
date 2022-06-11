using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class BookView : KaiaItemContentView
    {
        public BookView(BooksPage? From, CommandContext Context, string BookId, IEmote ReadPageEmote, bool CanGoBack) : base(From, Context, CanGoBack)
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
            return (await U.Settings.LibraryProcessor.GetUserBooksAsync()).FirstOrDefault(B => B.BookId == this.BookId) ?? KaiaLibrary.GetActiveBookById(this.BookId) ?? null;
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
                Embed.WriteField($"author", $"`{Book.Author}`");
                Embed.WriteField($"title", $"`{Book.Title}`");
                Embed.WriteField($"current page", $"`{Book.CurrentPageIndex}` / `{Book.Pages}`");
                Embed.WriteField($"current earnings", $"{Strings.Economy.CurrencyEmote} `{Book.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                if (Book.CurrentPageIndex <= Book.Pages)
                {
                    Embed.WriteField($"cost to read next page", $"{Strings.Economy.CurrencyEmote} `{Book.NextPageTurnCost}`");
                }
                Embed.WriteField($"your balance", $"{Strings.Economy.CurrencyEmote} `{U.Settings.Inventory.Petals}`");
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
            global::Discord.Rest.RestInteractionMessage? unused = await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
            this.Context.Reference.Client.ButtonExecuted += this.ButtonExecutedAsync;
        }

        private async Task ButtonExecutedAsync(global::Discord.WebSocket.SocketMessageComponent Component)
        {
            if ((Component.Data.CustomId == this.ReadNextPageId) && Component.User.Id == this.Context.UserContext.User.Id)
            {
                KaiaUser U = new(Component.User.Id);
                KaiaBook? Book = await this.GetUserBookAsync(U);
                if (Book != null && U.Settings.Inventory.Petals >= Book.NextPageTurnCost)
                {
                    if (!await U.Settings.LibraryProcessor.UserHasBookOfIdAsync(this.BookId) && KaiaLibrary.GetActiveBookById(this.BookId) is KaiaBook KBook)
                    {
                        await U.Settings.LibraryProcessor.AddBookAsync(KBook);
                    }
                    U.Settings.Inventory.Petals -= Book.NextPageTurnCost;
                    await U.Settings.LibraryProcessor.IncrementBookAsync(Book.BookId);
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
