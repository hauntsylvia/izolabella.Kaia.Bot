using izolabella.Storage.Objects.Structures;
using izolabella.Util;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Locations
{
    public class KaiaLocation : IDataStoreEntity
    {
        public KaiaLocation(string Name,
                            ulong SuperSecretSelfId,
                            IEnumerable<KeyValuePair<KaiaUserReward, int>> RewardsToChanceWeight,
                            TimeSpan AvailableAt,
                            TimeSpan AvailableTo,
                            TimeSpan MinimumTimeBetweenExplorations,
                            Uri CoverUrl)
        {
            this.Name = Name;
            this.Id = SuperSecretSelfId;
            this.RewardsToChanceWeight = RewardsToChanceWeight;
            this.AvailableAt = AvailableAt;
            this.AvailableTo = AvailableTo;
            this.MinimumTimeBetweenExplorations = MinimumTimeBetweenExplorations;
            this.CoverUrl = CoverUrl;
        }

        public string Name { get; }

        public IEnumerable<KeyValuePair<KaiaUserReward, int>> RewardsToChanceWeight { get; }

        public TimeSpan AvailableAt { get; }

        public TimeSpan AvailableTo { get; }

        public DateTime? AvailableFrom { get; set; }

        public TimeSpan MinimumTimeBetweenExplorations { get; }

        public DateTime? ExploredAt { get; }

        public Uri CoverUrl { get; }

        public ulong Id { get; }

        private KaiaUserReward GetReward()
        {
            int TotalWeight = this.RewardsToChanceWeight.Sum(A => A.Value);
            int Random = new Random().Next(0, TotalWeight);
            foreach(KeyValuePair<KaiaUserReward, int> RewardAndWeight in this.RewardsToChanceWeight)
            {
                Random -= RewardAndWeight.Value;
                if(Random <= 0)
                {
                    return RewardAndWeight.Key;
                }
            }
            return this.RewardsToChanceWeight.First().Key;
        }

        public async Task<KaiaLocationExplorationStatus> ExploreAsync(KaiaUser U)
        {
            if (this.ExploredAt.HasValue && (this.ExploredAt.Value.Add(this.MinimumTimeBetweenExplorations) < DateTime.UtcNow))
            {
                return KaiaLocationExplorationStatus.AttemptedExploreBeforeTimeoutWasOver;
            }
            if (this.AvailableFrom.HasValue && DateTime.UtcNow < this.AvailableFrom.Value)
            {
                return KaiaLocationExplorationStatus.LocationUnavailable;
            }
            else
            {
                KaiaUserReward Reward = this.GetReward();
                await U.Settings.Inventory.AddItemsToInventoryAndSaveAsync(U, Reward.Items);
                U.Settings.Inventory.Petals += Reward.Petals;
                return KaiaLocationExplorationStatus.Successful;
            }
        }
    }
}
