﻿using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Constants.Enums;
using Discord;
using System.Text.RegularExpressions;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class Leaderboard : ICCBCommand
    {
        public string Name => "Leaderboard";

        public string Description => "View a leaderboard.";

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
            IzolabellaCommandArgument? LeaderboardType = Arguments.FirstOrDefault(A => A.Name.ToLower() == "leaderboard");
            IzolabellaCommandArgument? LeaderboardAmount = Arguments.FirstOrDefault(A => A.Name.ToLower() == "display-amount");
            LeaderboardTypes LTypesMax = ((LeaderboardTypes[])Enum.GetValues(typeof(LeaderboardTypes))).Max();
            if (LeaderboardType != null && LeaderboardType.Value is long RawLType && (int)LTypesMax >= RawLType)
            {
                LeaderboardTypes LType = (LeaderboardTypes)RawLType;
                int AmountToDisplay = LeaderboardAmount != null && LeaderboardAmount.Value is long LVLong ? (int)LVLong : 10;
                await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new LeaderboardEmbed(
                    LType,
                    Context.Reference,
                    AmountToDisplay,
                    Leaderboard.GetNameOfLeaderboardType(LType)).Build());
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
                Choices.Add(new(Leaderboard.GetNameOfLeaderboardType(LType), (long)LType));
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

        public static string GetNameOfLeaderboardType(LeaderboardTypes LType)
        {
            string DisplayAfter = Regex.Replace(LType.ToString(), "([A-Z])", " $1");
            string CategoryDisplay = DisplayAfter.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries).First();
            DisplayAfter = DisplayAfter.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries).Last();
            string DisplayFull = $"[{CategoryDisplay}] {DisplayAfter}";
            return DisplayFull;
        }
    }
}