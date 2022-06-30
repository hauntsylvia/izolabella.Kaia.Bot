using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
{
    public class BooksPaginated : KaiaPathEmbedPaginated
    {
        public BooksPaginated(CommandContext Context, LibraryViewFilters Filter) : base(new(),
                                                          Context,
                                                          0,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Library)
        {
            KaiaUser User = new(Context.UserContext.User.Id);

            IEnumerable<KaiaBook> KaiasBooks = KaiaLibrary.Books.Where(KB => KB.AvailableUntil >= DateTime.UtcNow);
            IEnumerable<KaiaBook> UserBooks = User.LibraryProcessor.GetUserBooksAsync().Result;
            List<KaiaBook> Inventory = KaiasBooks.Where(B => UserBooks.All(K => K.BookId != B.BookId)).ToList();

            Inventory.AddRange(UserBooks);
            bool FirstPage = true;
            foreach (KaiaBook[] Chunk in Inventory.Where(IB =>
            {
                return Filter == LibraryViewFilters.Complete ? IB.IsFinished :
                       Filter == LibraryViewFilters.Incomplete ? !IB.IsFinished :
                       Filter == LibraryViewFilters.All;
            }).OrderBy(IB => IB.AvailableUntil).Chunk(2))
            {
                LibraryPage Embed = new(Chunk, User, FirstPage);
                if (FirstPage)
                {
                    FirstPage = false;
                }
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaBook Book in Chunk)
                {
                    B.Add(new(Book.Title, Book.BookId, $"by {Book.Author}", Emotes.Counting.Book, false));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            this.ItemSelected += this.ItemSelectedAsync;
        }

        private async void ItemSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            await Component.DeferAsync();
            BookView V = new(this, this.Context, ItemsSelected.FirstOrDefault() ?? "", Emotes.Counting.Book, true);
            await V.StartAsync(new(Component.User.Id));
            this.Dispose();
        }
    }
}
