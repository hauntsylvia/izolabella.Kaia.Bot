using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.User_Data
{
    internal class MeLibraryView : CCBPathPaginatedEmbed
    {
        public MeLibraryView(KaiaUser User, CommandContext Context, int LibraryChunkSize) : base(new(),
                                                          new(Strings.EmbedStrings.PathIfNoGuild,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username),
                                                          Context,
                                                          0,
                                                          Emotes.Embeds.Back,
                                                          Emotes.Embeds.Forward,
                                                          Strings.EmbedStrings.PathIfNoGuild,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username)
        {
            MeView LandingPage = new(Context.UserContext.User.Username, User);
            List<KaiaBook> Books = User.Settings.LibraryProcessor.GetUserBooksAsync().Result;
            IEnumerable<KaiaBook[]> BookChunked = Books.Chunk(LibraryChunkSize);
            LandingPage.WriteField($"{Emotes.Counting.Book} library", $"`{Books.Count}` {(Books.Count == 1 ? "book" : "books")}");
            this.EmbedsAndOptions.Add(LandingPage, null);
            foreach (KaiaBook[] Chunk in BookChunked)
            {
                List<string> Display = new();
                CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username);
                foreach (KaiaBook Item in Chunk)
                {
                    Display.Add($"`{Item.Title}` by `{Item.Author}` - currently on page `{Item.CurrentPageIndex}` out of `{Item.Pages}`\n{Strings.Economy.CurrencyEmote} `{Item.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                }
                Embed.WriteListToOneField("library", Display, "\n\n");
                this.EmbedsAndOptions.Add(Embed, null);
            }
        }
    }
}
