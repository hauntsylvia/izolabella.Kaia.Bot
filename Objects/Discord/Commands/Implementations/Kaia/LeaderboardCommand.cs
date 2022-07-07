using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Others;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Kaia
{
    public class LeaderboardCommand : KaiaCommand
    {
        public override string Name => "Leaderboard";

        public override string Description => "View a leaderboard.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters { get; } = new();

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override string ForeverId => CommandForeverIds.ViewLeaderboard;

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? LeaderboardType = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "leaderboard");
            LeaderboardTypes LType = EnumToReadable.GetEnumFromArg<LeaderboardTypes>(LeaderboardType);
            await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new LeaderboardEmbed(
                LType,
                Context.Reference,
                10,
                EnumToReadable.GetNameOfEnumType(LType)).Build());
        }

        public override Task OnLoadAsync(IzolabellaCommand[] AllCommands)
        {
            this.Parameters.Add(EnumToReadable.MakeChoicesFromEnum("Leaderboard", "The leaderboard to view.", typeof(LeaderboardTypes)));
            return Task.CompletedTask;
        }
    }
}
