using Discord;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Embeds.Implementations;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData;
using Kaia.Bot.Objects.Util;
using System.Text.RegularExpressions;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class LeaderboardCommand : IKaiaCommand
    {
        public string Name => "Leaderboard";

        public string Description => "View a leaderboard.";
        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Display Amount", "The number of elements in the leaderboard to display.", ApplicationCommandOptionType.Integer, false)
            {
                MaxValue = 30,
                MinimumValue = 1
            }
        };
        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.ViewLeaderboard;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? LeaderboardType = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "leaderboard");
            IzolabellaCommandArgument? LeaderboardAmount = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "display-amount");
            LeaderboardTypes LTypesMax = ((LeaderboardTypes[])Enum.GetValues(typeof(LeaderboardTypes))).Max();
            if (LeaderboardType != null && LeaderboardType.Value is long RawLType && (int)LTypesMax >= RawLType)
            {
                LeaderboardTypes LType = (LeaderboardTypes)RawLType;
                int AmountToDisplay = LeaderboardAmount != null && LeaderboardAmount.Value is long LVLong ? (int)LVLong : 10;
                await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new LeaderboardEmbed(
                    LType,
                    Context.Reference,
                    AmountToDisplay,
                    EnumToReadable.GetNameOfEnumType(LType)).Build());
            }
            else
            {
                await Context.UserContext.RespondAsync(text: Strings.Responses.Commands.InvalidLeaderboardOption, ephemeral: true);
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            List<IzolabellaCommandParameterChoices> Choices = new();
            foreach (LeaderboardTypes LType in Enum.GetValues(typeof(LeaderboardTypes)))
            {
                Choices.Add(new(EnumToReadable.GetNameOfEnumType(LType), (long)LType));
            }
            this.Parameters.Add(new("Leaderboard", "The leaderboard to view.", ApplicationCommandOptionType.Integer, true)
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
