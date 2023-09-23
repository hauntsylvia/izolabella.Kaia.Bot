using izolabella.Kaia.Bot.Objects.Constants.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Storage.Objects.Structures;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases
{
    public abstract class KaiaAchievement(ulong Id, string Title, string DescriptionWhenAchieved, string DescriptionWhenNotAchieved, AchievementCategory Category, KaiaEmote DisplayEmote, params KaiaUserReward[] Rewards) : IDataStoreEntity
    {
        public ulong Id { get; } = Id;

        public string Title { get; } = Title;

        [JsonProperty("WhenAchieved")]
        private string DescriptionWhenAchieved { get; } = DescriptionWhenAchieved;

        [JsonProperty("WhenNotAchieved")]
        private string DescriptionWhenNotAchieved { get; } = DescriptionWhenNotAchieved;

        public AchievementCategory Category { get; } = Category;

        public KaiaUserReward[] Rewards { get; } = Rewards;

        public DateTime InitializedAt { get; } = DateTime.UtcNow;

        public KaiaEmote DisplayEmote { get; } = DisplayEmote;

        public abstract Task<bool> CanAwardAsync(KaiaUser U, CommandContext? Context);

        public async Task<bool> UserAlreadyOwns(KaiaUser U)
        {
            return (await U.AchievementProcessor.GetUserAchievementsAsync()).Any(X => X.Id == this.Id);
        }

        public async Task<string> GetDescriptionAsync(KaiaUser U)
        {
            return await this.UserAlreadyOwns(U) ? this.DescriptionWhenAchieved : this.DescriptionWhenNotAchieved;
        }
    }
}