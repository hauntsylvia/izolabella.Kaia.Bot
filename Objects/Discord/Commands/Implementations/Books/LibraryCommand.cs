using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Constants.Enums;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Books;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Books
{
    public class KaiaLibraryCommand : KaiaCommand
    {
        public override string Name => "Library";

        public override string Description => "View Kaia's library.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters { get; } = new();

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override string ForeverId => CommandForeverIds.BookLibrary;

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
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

        public override Task OnLoadAsync(IzolabellaCommand[] AllCommands)
        {
            this.Parameters.Add(EnumToReadable.MakeChoicesFromEnum("Book Filter", "The filter to apply the books by.", typeof(LibraryViewFilters)));
            return Task.CompletedTask;
        }
    }
}