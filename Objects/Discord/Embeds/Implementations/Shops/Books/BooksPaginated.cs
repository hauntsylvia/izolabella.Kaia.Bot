using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Constants.Enums;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books
{
    public class BooksPaginated : KaiaPathEmbedPaginated
    {
        public BooksPaginated(CommandContext Context, LibraryViewFilters Filter) : base(new(),
                                                          Context,
                                                          0,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Library)
        {
            this.Filter = Filter;
            ItemSelected += this.ItemSelectedAsync;
        }

        public LibraryViewFilters Filter { get; }

        protected override async Task ClientRefreshAsync()
        {
            KaiaUser User = new(this.Context.UserContext.User.Id);

            IEnumerable<KaiaBook> KaiasBooks = KaiaLibrary.Books.Where(KB => KB.AvailableUntil >= DateTime.UtcNow);
            IEnumerable<KaiaBook> UserBooks = await User.LibraryProcessor.GetUserBooksAsync();
            List<KaiaBook> Inventory = KaiasBooks.Where(B => UserBooks.All(K => K.BookId != B.BookId)).ToList();

            Inventory.AddRange(UserBooks);
            bool FirstPage = true;
            foreach (KaiaBook[] Chunk in Inventory.Where(IB =>
            {
                return this.Filter == LibraryViewFilters.Complete ? IB.IsFinished :
                       this.Filter == LibraryViewFilters.Incomplete ? !IB.IsFinished :
                       this.Filter == LibraryViewFilters.All;
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