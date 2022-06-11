using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;

namespace Kaia.Bot.Objects.KaiaStructures.Achievements.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserAchievementRoom
    {
        public UserAchievementRoom(ulong U)
        {
            this.U = U;
            this.Process = DataStores.GetUserAchievementStore(this.U);
        }

        public ulong U { get; }

        public DataStore Process { get; }

        public delegate void AchievementAwardedHandler(KaiaUser User, CommandContext? CanRespondTo, List<KaiaAchievement> Achievements);

        public event AchievementAwardedHandler? AchievementAwarded;

        public async Task<List<KaiaAchievement>> GetUserAchievementsAsync()
        {
            return await this.Process.ReadAllAsync<KaiaAchievement>();
        }

        public async Task TryAwardAchievements(KaiaUser User, CommandContext? CanRespondTo, params KaiaAchievement[] Achievements)
        {
            List<KaiaAchievement> Actual = new();
            foreach (KaiaAchievement A in Achievements)
            {
                if (await A.CanAwardAsync(User, CanRespondTo) && !await A.UserAlreadyOwns(User))
                {
                    await this.Process.SaveAsync(A);
                    User.Settings.Inventory.Petals += A.Rewards.Sum(AA =>
                    {
                        return AA.Petals;
                    });
                    foreach (KaiaAchievementReward R in A.Rewards)
                    {
                        User.Settings.Inventory.Items.AddRange(R.Items);
                    }
                    Actual.Add(A);
                }
            }
            this.AchievementAwarded?.Invoke(User, CanRespondTo, Actual);
        }
    }
}
