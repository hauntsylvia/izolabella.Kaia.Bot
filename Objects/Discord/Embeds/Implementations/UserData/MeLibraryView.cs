using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    internal class MeLibraryView : KaiaPathEmbedPaginated
    {
        public MeLibraryView(KaiaUser User, CommandContext Context, int LibraryChunkSize) : base(new(),
                                                          new(Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username),
                                                          Context,
                                                          0,
                                                          Strings.EmbedStrings.FakePaths.Global,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username)
        {
            MeView LandingPage = new(Context.UserContext.User.Username, User);
            List<KaiaBook> Books = User.Settings.LibraryProcessor.GetUserBooksAsync().Result;
            IEnumerable<KaiaBook[]> BookChunked = Books.Chunk(LibraryChunkSize);
            LandingPage.WithField($"{Emotes.Counting.Book} library", $"`{Books.Count}` {(Books.Count == 1 ? "book" : "books")}");
            this.EmbedsAndOptions.Add(LandingPage, null);
            foreach (KaiaBook[] Chunk in BookChunked)
            {
                List<string> Display = new();
                KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username);
                foreach (KaiaBook Item in Chunk)
                {
                    Display.Add($"`{Item.Title}` by `{Item.Author}` - currently on page `{Item.CurrentPageIndex}` out of `{Item.Pages}`\n{Strings.Economy.CurrencyEmote} `{Item.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                }
                Embed.WithListWrittenToField("library", Display, "\n\n");
                this.EmbedsAndOptions.Add(Embed, null);
            }
        }
    }
}
