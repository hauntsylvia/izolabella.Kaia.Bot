using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    internal class ViewAchievementsCommand : IKaiaCommand
    {
        public string ForeverId => CommandForeverIds.ViewAchievements;

        public string Name => "Achievements";

        public string Description => "View your achievements.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new();

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            this.Parameters.Add(EnumToReadable.MakeChoicesFromEnum("Achievement Filter", "The filter to apply to the list of achievements.", typeof(AchievementFilter)));
            return Task.CompletedTask;
        }

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? FilterArg = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "achievement-filter");
            AchievementFilter Result = EnumToReadable.GetEnumFromArg<AchievementFilter>(FilterArg);
            await new AchievementsPaginated(Context, Result, 4).StartAsync();
        }
    }
}
