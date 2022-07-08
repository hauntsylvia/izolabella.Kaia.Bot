using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Achievements
{
    internal class ViewAchievementsCommand : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.ViewAchievements;

        public override string Name => "Achievements";

        public override string Description => "View your achievements.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters { get; } = new();

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override Task OnLoadAsync(IzolabellaCommand[] AllCommands)
        {
            this.Parameters.Add(EnumToReadable.MakeChoicesFromEnum("Achievement Filter", "The filter to apply to the list of achievements.", typeof(AchievementFilter)));
            return Task.CompletedTask;
        }

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? FilterArg = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == "achievement-filter");
            AchievementFilter Result = EnumToReadable.GetEnumFromArg<AchievementFilter>(FilterArg);
            await new AchievementsPaginated(Context, Result, 4).StartAsync();
        }
    }
}
