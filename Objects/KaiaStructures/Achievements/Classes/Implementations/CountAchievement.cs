using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations;

public class CountAchievement : KaiaAchievement
{
    public CountAchievement(ulong Id,
                            ulong CountTo,
                            string WhenAchieved,
                            string WhenNotAchieved,
                            bool Highest) : base(Id,
                                                 $"{(Highest ? $"Highest Number Counted - " : $"Total Numbers Counted - ")}{CountTo.ToString(CultureInfo.InvariantCulture)}",
                                                 WhenAchieved,
                                                 WhenNotAchieved,
                                                 Constants.Enums.AchievementCategory.Counting,
                                                 Emotes.Achievements.Counting,
                                                 new KaiaUserReward[] { new(CountTo) })
    {
        this.CountTo = CountTo;
        this.Highest = Highest;
    }

    public ulong CountTo { get; }
    public bool Highest { get; }

    public override Task<bool> CanAwardAsync(KaiaUser U, CommandContext? Context)
    {
        return Task.FromResult((this.Highest ? U.Settings.HighestCountEver ?? 0 : U.Settings.NumbersCounted ?? 0) >= this.CountTo);
    }
}
