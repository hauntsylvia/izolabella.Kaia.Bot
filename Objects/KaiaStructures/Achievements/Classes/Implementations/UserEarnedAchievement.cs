using izolabella.Kaia.Bot.Objects.Constants.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations
{
    [method: JsonConstructor]/// <summary>
                             /// An achievement already earned by the user.
                             /// </summary>
                             /// <param name="Id"></param>
                             /// <param name="Title"></param>
                             /// <param name="Description"></param>
                             /// <param name="Category"></param>
                             /// <param name="DisplayEmote"></param>
                             /// <param name="Rewards"></param>
    public class UserEarnedAchievement(ulong Id, string Title, string DescriptionWhenAchieved, string DescriptionWhenNotAchieved, AchievementCategory Category, KaiaEmote DisplayEmote, params KaiaUserReward[] Rewards) : KaiaAchievement(Id, Title, DescriptionWhenAchieved, DescriptionWhenNotAchieved, Category, DisplayEmote, Rewards)
    {
        public override Task<bool> CanAwardAsync(KaiaUser U, CommandContext? Context)
        {
            return Task.FromResult(false);
        }
    }
}