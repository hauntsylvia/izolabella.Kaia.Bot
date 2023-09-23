using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations
{
    public class CountAchievement(ulong Id,
                            ulong CountTo,
                            string WhenAchieved,
                            string WhenNotAchieved,
                            bool Highest) : KaiaAchievement(Id,
                                                 $"{(Highest ? $"Highest Number Counted - " : $"Total Numbers Counted - ")}{CountTo.ToString(CultureInfo.InvariantCulture)}",
                                                 WhenAchieved,
                                                 WhenNotAchieved,
                                                 Constants.Enums.AchievementCategory.Counting,
                                                 Emotes.Achievements.Counting,
                                                 new KaiaUserReward[] { new(CountTo) })
    {
        public ulong CountTo { get; } = CountTo;
        public bool Highest { get; } = Highest;

        public override Task<bool> CanAwardAsync(KaiaUser U, CommandContext? Context)
        {
            return Task.FromResult((this.Highest ? U.Settings.HighestCountEver ?? 0 : U.Settings.NumbersCounted ?? 0) >= this.CountTo);
        }
    }
}