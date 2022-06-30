using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations;

namespace Kaia.Bot.Objects.KaiaStructures.Achievements.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserAchievementRoom
    {
        public UserAchievementRoom(ulong U)
        {
            this.U = U;
            if(this.U != default)
            {
                this.UserAchievementsStore = DataStores.GetUserAchievementStore(this.U);
            }
        }

        public ulong U { get; }

        private DataStore? UserAchievementsStore { get; }

        public delegate void AchievementAwardedHandler(KaiaUser User, CommandContext? CanRespondTo, List<KaiaAchievement> Achievements);

        public event AchievementAwardedHandler? AchievementsRewarded;

        public async Task<IReadOnlyCollection<UserEarnedAchievement>> GetUserAchievementsAsync()
        {
            return await (this.UserAchievementsStore != null ? this.UserAchievementsStore.ReadAllAsync<UserEarnedAchievement>() : Task.FromResult(new List<UserEarnedAchievement>()));
        }

        public async Task TryAwardAchievements(KaiaUser User, CommandContext? CanRespondTo, params KaiaAchievement[] Achievements)
        {
            if(this.UserAchievementsStore != null)
            {
                List<KaiaAchievement> Actual = new();
                foreach (KaiaAchievement A in Achievements)
                {
                    if (await A.CanAwardAsync(User, CanRespondTo) && !await A.UserAlreadyOwns(User))
                    {
                        await this.UserAchievementsStore.SaveAsync(A);
                        User.Settings.Inventory.Petals += A.Rewards.Sum(AA => AA.Petals);
                        foreach (KaiaUserReward R in A.Rewards)
                        {
                            await User.Settings.Inventory.AddItemsToInventoryAndSaveAsync(User, R.Items);
                        }
                        Actual.Add(A);
                    }
                }
                if (Actual.Count > 0)
                {
                    this.AchievementsRewarded?.Invoke(User, CanRespondTo, Actual);
                }
            }
        }
    }
}
