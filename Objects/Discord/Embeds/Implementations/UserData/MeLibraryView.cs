using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    public class MeLibraryView : KaiaPathEmbedPaginated
    {
        public MeLibraryView(KaiaUser User, CommandContext Context, int LibraryChunkSize) : base(new(),
                                                          Context,
                                                          0,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username)
        {
            MeView LandingPage = new(Context.UserContext.User.Username, User);
            List<KaiaBook> Books = User.LibraryProcessor.GetUserBooksAsync().Result;
            IEnumerable<KaiaBook[]> BookChunked = Books.Chunk(LibraryChunkSize);
            LandingPage.WithField($"{Emotes.Counting.Book} library", $"`{Books.Count}` {(Books.Count == 1 ? "book" : "books")}");
            this.EmbedsAndOptions.Add(LandingPage, null);
            foreach (KaiaBook[] Chunk in BookChunked)
            {
                LibraryPage Embed = new(Chunk, User, false);
                this.EmbedsAndOptions.Add(Embed, null);
            }
        }
    }
}
