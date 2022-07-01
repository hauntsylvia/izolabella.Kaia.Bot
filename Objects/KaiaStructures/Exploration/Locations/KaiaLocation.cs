using izolabella.Storage.Objects.Structures;
using izolabella.Util;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Locations
{
    public class KaiaLocation : IDataStoreEntity
    {
        public KaiaLocation(string Name,
                            string Description,
                            string ShortDescription,
                            bool DisplayRewards,
                            ulong SuperSecretSelfId,
                            IEnumerable<KaiaLocationEvent> Events,
                            TimeSpan AvailableAt,
                            TimeSpan AvailableTo,
                            TimeSpan MinimumTimeBetweenExplorations,
                            Uri CoverUrl,
                            DateTime? ExploredAt = null)
        {
            this.Name = Name;
            this.Description = Description;
            this.ShortDescription = ShortDescription;
            this.DisplayRewards = DisplayRewards;
            this.Id = SuperSecretSelfId;
            this.Events = Events;
            this.AvailableAt = AvailableAt;
            this.AvailableTo = AvailableTo;
            this.MinimumTimeBetweenExplorations = MinimumTimeBetweenExplorations;
            this.CoverUrl = CoverUrl;
            this.ExploredAt = ExploredAt;
        }

        public string Name { get; }

        public string Description { get; }

        public string ShortDescription { get; }

        public bool DisplayRewards { get; }

        public IEnumerable<KaiaLocationEvent> Events { get; }

        public TimeSpan AvailableAt { get; }

        public TimeSpan AvailableTo { get; }

        public DateTime? AvailableUntil { get; set; }

        public TimeSpan MinimumTimeBetweenExplorations { get; }

        public DateTime? ExploredAt { get; private set; }

        public DateTime? MustWaitUntil => this.ExploredAt.HasValue ? this.ExploredAt.Value.Add(this.MinimumTimeBetweenExplorations) : null;

        public Uri CoverUrl { get; }

        public bool Overnight { get; set; }

        [JsonProperty("SuperSecretSelfId")]
        public ulong Id { get; }

        public KaiaLocationExplorationStatus Status =>
            !this.Overnight && (DateTime.UtcNow.TimeOfDay < this.AvailableAt || DateTime.UtcNow.TimeOfDay > this.AvailableTo) ? KaiaLocationExplorationStatus.IncorrectLocationTime :
            this.Overnight && (DateTime.UtcNow.TimeOfDay > this.AvailableAt || DateTime.UtcNow.Add(this.AvailableTo).TimeOfDay < this.AvailableTo) ? KaiaLocationExplorationStatus.IncorrectLocationTime :
            this.AvailableUntil.HasValue && DateTime.UtcNow < this.AvailableUntil.Value ? KaiaLocationExplorationStatus.LocationUnavailable :
            this.MustWaitUntil.HasValue && (this.MustWaitUntil.Value > DateTime.UtcNow) ? KaiaLocationExplorationStatus.Timeout :
            KaiaLocationExplorationStatus.Successful;

        public KaiaLocationExplorationStatus StatusNoTimeout => this.Status == KaiaLocationExplorationStatus.Timeout ? KaiaLocationExplorationStatus.Successful : this.Status;

        private KaiaLocationEvent GetEvent()
        {
            int TotalWeight = this.Events.Sum(A => A.Weight);
            int Random = new Random().Next(0, TotalWeight);
            foreach(KaiaLocationEvent Event in this.Events)
            {
                Random -= Event.Weight;
                if(Random <= 0)
                {
                    return Event;
                }
            }
            return this.Events.First();
        }

        public async Task<KaiaLocationEvent> ExploreAsync(KaiaUser U)
        {
            KaiaLocationEvent Event = this.GetEvent();
            Event.Status = this.Status;
            if (this.Status == KaiaLocationExplorationStatus.Successful)
            {
                this.ExploredAt = DateTime.UtcNow;
                await U.Settings.Inventory.AddItemsToInventoryAndSaveAsync(U, Event.RewardResult.Items);
                U.Settings.Inventory.Petals += Event.RewardResult.Petals;
                return Event;
            }
            else
            {
                return Event;
            }
        }
    }
}
