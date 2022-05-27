using Discord;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Implementations;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class KaiaLibraryCommand : IKaiaCommand
    {
        public string Name => "Library";

        public string Description => "View Kaia's library.";

        public List<IzolabellaCommandParameter> Parameters { get; } = new();

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.BookLibrary;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? BookArg = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "book");
            if (BookArg != null && BookArg.Value is string BookId && KaiaLibrary.GetActiveBookById(BookId) is KaiaBook Book)
            {
                await new BookView(Context, Book.BookId, Emotes.Counting.Book).StartAsync(new(Context.UserContext.User.Id));
            }
            else if (BookArg != null)
            {
                await Context.UserContext.RespondAsync(text: Strings.Responses.Commands.NoBookFound);
            }
            else
            {
                await new BooksPage(Context, 5).StartAsync();
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            List<IzolabellaCommandParameterChoices> Choices = new();
            foreach (KaiaBook Book in KaiaLibrary.Books)
            {
                Choices.Add(new(Book.Title, Book.BookId));
            }
            this.Parameters.Add(new("Book", "The book to view.", ApplicationCommandOptionType.String, false)
            {
                Choices = Choices
            });
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
