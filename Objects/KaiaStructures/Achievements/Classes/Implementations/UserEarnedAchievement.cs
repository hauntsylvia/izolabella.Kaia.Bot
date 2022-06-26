using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties;

namespace Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations
{
    public class UserEarnedAchievement : KaiaAchievement
    {

        /// <summary>
        /// An achievement already earned by the user.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <param name="Category"></param>
        /// <param name="DisplayEmote"></param>
        /// <param name="Rewards"></param>
        [JsonConstructor]
        public UserEarnedAchievement(ulong Id, string Title, string DescriptionWhenAchieved, string DescriptionWhenNotAchieved, AchievementCategory Category, KaiaItemEmote DisplayEmote, params KaiaAchievementReward[] Rewards) : base(Id, Title, DescriptionWhenAchieved, DescriptionWhenNotAchieved, Category, DisplayEmote, Rewards)
        {
        }

        public override Task<bool> CanAwardAsync(KaiaUser U, CommandContext? Context)
        {
            return Task.FromResult(false);
        }
    }
}
