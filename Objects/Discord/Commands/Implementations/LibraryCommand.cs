using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books;
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
            //LibraryViewFilters LFilterMax = ((LibraryViewFilters[])Enum.GetValues(typeof(LibraryViewFilters))).Max();
            //LibraryViewFilters Result = LibraryViewFilters.ShowAll;
            //if(BookFilterArg != null && BookFilterArg.Value is long RawFilter && (int)LFilterMax >= RawFilter)
            //{
            //    Result = (LibraryViewFilters)RawFilter;
            //}
            LibraryViewFilters Result = EnumToReadable.GetEnumFromArg<LibraryViewFilters>(BookFilterArg);
            await new BooksPaginated(Context, Result).StartAsync();
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            this.Parameters.Add(EnumToReadable.MakeChoicesFromEnum("Book Filter", "The filter to apply the books by.", typeof(LibraryViewFilters)));
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
