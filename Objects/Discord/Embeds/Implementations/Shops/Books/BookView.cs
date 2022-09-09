using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books;

public class BookView : KaiaItemContentView
{
    public BookView(BooksPaginated? From, CommandContext Context, string BookId, IEmote ReadPageEmote, bool CanGoBack) : base(From, Context, CanGoBack)
    {
        this.ReadNextPageButton = new(Context, "Read", ReadPageEmote);
        this.ReadNextPageButton.OnButtonPush += this.ReadNextPageAsync;
        this.BookId = BookId;
    }

    public KaiaButton ReadNextPageButton { get; }

    public string BookId { get; }

    private async Task ReadNextPageAsync(SocketMessageComponent Component, KaiaUser U)
    {
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
        KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
        ComponentBuilder Com = await this.GetComponentsAsync(U);
        await Component.UpdateAsync(C =>
        {
            C.Embed = E.Build();
            C.Components = Com.Build();
        });
    }

    private async Task<KaiaBook?> GetUserBookAsync(KaiaUser U)
    {
        return (await U.LibraryProcessor.GetUserBooksAsync()).FirstOrDefault(B => B.BookId == this.BookId) ?? KaiaLibrary.GetActiveBookById(this.BookId) ?? null;
    }

    public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
    {
        KaiaBook? Book = await this.GetUserBookAsync(U);
        ComponentBuilder CB = (await this.GetDefaultComponents()).WithButton(this.ReadNextPageButton.WithDisabled(Book != null && Book.CurrentPageIndex >= Book.Pages));
        return CB;
    }

    public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
    {
        KaiaBook? Book = await this.GetUserBookAsync(U);
        KaiaPathEmbedRefreshable Embed = Book != null ? new BookRawView(Book, U) : new SingleItemNotFound();
        await Embed.RefreshAsync();
        return Embed;
    }

    public override async Task StartAsync(KaiaUser U)
    {
        if (!this.Context.UserContext.HasResponded)
        {
            await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
        }
        KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
        ComponentBuilder Com = await this.GetComponentsAsync(U);
        _ = await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
        {
            M.Content = Strings.EmbedStrings.Empty;
            M.Components = Com.Build();
            M.Embed = E.Build();
        });
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        this.ReadNextPageButton.OnButtonPush -= this.ReadNextPageAsync;
    }
}
