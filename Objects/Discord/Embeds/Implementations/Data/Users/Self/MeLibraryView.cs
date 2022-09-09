using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeData;

public class MeLibraryView : KaiaPathEmbedPaginated
{
    public MeLibraryView(KaiaUser User, CommandContext Context, int LibraryChunkSize) : base(new(),
                                                      Context,
                                                      0,
                                                      Strings.EmbedStrings.FakePaths.Global,
                                                      Strings.EmbedStrings.FakePaths.Users,
                                                      Context.UserContext.User.Username)
    {
        this.User = User;
        this.LibraryChunkSize = LibraryChunkSize;
    }

    public KaiaUser User { get; }

    public int LibraryChunkSize { get; }

    protected override async Task ClientRefreshAsync()
    {
        MeView LandingPage = new(this.Context.UserContext.User.Username, this.User);
        List<KaiaBook> Books = await this.User.LibraryProcessor.GetUserBooksAsync();
        IEnumerable<KaiaBook[]> BookChunked = Books.Chunk(this.LibraryChunkSize);
        LandingPage.WithField($"{Emotes.Counting.Book} library", $"`{Books.Count}` {(Books.Count == 1 ? "book" : "books")}");
        this.EmbedsAndOptions.Add(LandingPage, null);
        foreach (KaiaBook[] Chunk in BookChunked)
        {
            LibraryPage Embed = new(Chunk, this.User, false);
            this.EmbedsAndOptions.Add(Embed, null);
        }
    }
}
