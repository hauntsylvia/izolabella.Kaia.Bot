using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Storage.Objects.DataStores;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserAchievementRoom
    {
        public UserAchievementRoom(ulong U)
        {
            this.U = U;
            if (this.U != default)
            {
                UserAchievementsStore = DataStores.GetUserAchievementStore(this.U);
            }
        }

        public ulong U { get; }

        private DataStore? UserAchievementsStore { get; }

        public delegate void AchievementAwardedHandler(KaiaUser User, CommandContext? CanRespondTo, List<KaiaAchievement> Achievements);

        public event AchievementAwardedHandler? AchievementsRewarded;

        public async Task<IReadOnlyCollection<UserEarnedAchievement>> GetUserAchievementsAsync()
        {
            return await (UserAchievementsStore != null ? UserAchievementsStore.ReadAllAsync<UserEarnedAchievement>() : Task.FromResult(new List<UserEarnedAchievement>()));
        }

        public async Task TryAwardAchievements(KaiaUser User, CommandContext? CanRespondTo, params KaiaAchievement[] Achievements)
        {
            if (UserAchievementsStore != null)
            {
                List<KaiaAchievement> Actual = new();
                foreach (KaiaAchievement A in Achievements)
                {
                    if (await A.CanAwardAsync(User, CanRespondTo) && !await A.UserAlreadyOwns(User))
                    {
                        await UserAchievementsStore.SaveAsync(A);
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
                    AchievementsRewarded?.Invoke(User, CanRespondTo, Actual);
                }
            }
        }
    }
}
