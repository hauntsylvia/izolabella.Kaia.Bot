using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class LeaderboardCommand : IKaiaCommand
    {
        public string Name => "Leaderboard";

        public string Description => "View a leaderboard.";
        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new();
        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.ViewLeaderboard;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? LeaderboardType = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "leaderboard");
            LeaderboardTypes LType = EnumToReadable.GetEnumFromArg<LeaderboardTypes>(LeaderboardType);
            await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new LeaderboardEmbed(
                LType,
                Context.Reference,
                10,
                EnumToReadable.GetNameOfEnumType(LType)).Build());
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            this.Parameters.Add(EnumToReadable.MakeChoicesFromEnum("Leaderboard", "The leaderboard to view.", typeof(LeaderboardTypes)));
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
