using Discord;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class KaiaLibraryCommand : IKaiaCommand
    {
        public string Name => "Library";

        public string Description => "View Kaia's library.";
        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.BookLibrary;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? BookFilterArg = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "book-filter");
            LibraryViewFilters LFilterMax = ((LibraryViewFilters[])Enum.GetValues(typeof(LibraryViewFilters))).Max();
            LibraryViewFilters Result = LibraryViewFilters.ShowAll;
            if(BookFilterArg != null && BookFilterArg.Value is long RawFilter && (int)LFilterMax >= RawFilter)
            {
                Result = (LibraryViewFilters)RawFilter;
            }
            await new BooksPage(Context, Result).StartAsync();
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            List<IzolabellaCommandParameterChoices> Choices = new();
            foreach (LibraryViewFilters Filter in Enum.GetValues(typeof(LibraryViewFilters)))
            {
                Choices.Add(new(EnumToReadable.GetNameOfEnumType(Filter), (long)Filter));
            }
            this.Parameters.Add(new("Book Filter", "How to filter the books.", ApplicationCommandOptionType.Integer, false)
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
