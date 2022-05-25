using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.User_Data
{
    internal class MeLibrary : CCBPathPaginatedEmbed
    {
        public MeLibrary(CCBUser User, CommandContext Context, int LibraryChunkSize) : base(new(),
                                                          Context,
                                                          0,
                                                          Emotes.Embeds.Back,
                                                          Emotes.Embeds.Forward,
                                                          Strings.EmbedStrings.PathIfNoGuild,
                                                          Strings.EmbedStrings.FakePaths.Users,
                                                          Context.UserContext.User.Username)
        {
            MeView LandingPage = new(Context.UserContext.User.Username, User);
            IEnumerable<KaiaBook[]> BookChunked = User.Settings.Library.Items.Chunk(LibraryChunkSize);
            LandingPage.WriteField($"{Emotes.Counting.Book}", $"`{User.Settings.Library.Items.Count}` books");
            this.Embeds.Add(LandingPage);
            foreach (KaiaBook[] Chunk in BookChunked)
            {
                List<string> Display = new();
                CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username);
                foreach (KaiaBook Item in Chunk)
                {
                    Display.Add($"{Item.Title} by {Item.Author} - currently on page {Item.CurrentPage}");
                }
                Embed.WriteListToOneField("inventory", Display, "\n");
                this.Embeds.Add(Embed);
            }
        }
    }
}
