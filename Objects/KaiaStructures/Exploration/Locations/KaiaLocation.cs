using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Storage.Objects.Structures;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations
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
                            KaiaEmote Emote,
                            Uri? CoverUrl = null,
                            Uri? CoverUrlCredit = null,
                            DateTime? ExploredAt = null)
        {
            this.Name = Name;
            this.Description = Description;
            this.ShortDescription = ShortDescription;
            displayRewards = DisplayRewards;
            Id = SuperSecretSelfId;
            this.Events = Events;
            this.AvailableAt = AvailableAt;
            this.AvailableTo = AvailableTo;
            this.MinimumTimeBetweenExplorations = MinimumTimeBetweenExplorations;
            this.Emote = Emote;
            this.CoverUrl = CoverUrl;
            this.CoverUrlCredit = CoverUrlCredit;
            this.ExploredAt = ExploredAt;
        }

        public string Name { get; }

        public string Description { get; }

        public string ShortDescription { get; }

        private readonly bool displayRewards;

        public bool DisplayRewards => displayRewards && Events.Any();

        public IEnumerable<KaiaLocationEvent> Events { get; }

        public TimeSpan AvailableAt { get; }

        public TimeSpan AvailableTo { get; }

        public DateTime? AvailableUntil { get; set; }

        public TimeSpan MinimumTimeBetweenExplorations { get; }

        public KaiaEmote Emote { get; }

        public DateTime? ExploredAt { get; private set; }

        public DateTime? MustWaitUntil => ExploredAt.HasValue ? ExploredAt.Value.Add(MinimumTimeBetweenExplorations) : null;

        public Uri? CoverUrl { get; }

        public Uri? CoverUrlCredit { get; }

        public bool Overnight => AvailableAt > AvailableTo;

        public DateTime InnerClock { get; } = DateTime.UtcNow;

        public DateTime Start => InnerClock.Date.Add(AvailableAt);

        public DateTime End => Overnight ? InnerClock.Date.AddDays(1).Add(AvailableTo) : InnerClock.Date.Add(AvailableTo);

        [JsonProperty("SuperSecretSelfId")]
        public ulong Id { get; }

        public KaiaLocationExplorationStatus Status =>
            Overnight && InnerClock.TimeOfDay > AvailableTo && InnerClock.TimeOfDay < AvailableAt ? KaiaLocationExplorationStatus.IncorrectLocationTime :
            !Overnight && (InnerClock < Start || InnerClock > End) ? KaiaLocationExplorationStatus.IncorrectLocationTime :
            AvailableUntil.HasValue && InnerClock < AvailableUntil.Value ? KaiaLocationExplorationStatus.LocationUnavailable :
            MustWaitUntil.HasValue && MustWaitUntil.Value > InnerClock ? KaiaLocationExplorationStatus.Timeout :
            KaiaLocationExplorationStatus.Successful;

        public KaiaLocationExplorationStatus StatusNoTimeout => Status == KaiaLocationExplorationStatus.Timeout ? KaiaLocationExplorationStatus.Successful : Status;

        private KaiaLocationEvent GetEvent()
        {
            double TotalWeight = Events.Sum(A => A.Weight);
            double Random = new Random().Next(0, (int)TotalWeight) + TotalWeight - (int)TotalWeight;
            foreach (KaiaLocationEvent Event in Events)
            {
                Random -= Event.Weight;
                if (Random <= 0)
                {
                    return Event;
                }
            }
            return Events.First();
        }

        public async Task<KaiaLocationEvent> ExploreAsync(KaiaUser U)
        {
            KaiaLocationEvent Event = GetEvent();
            Event.Status = Status;
            if (Status == KaiaLocationExplorationStatus.Successful)
            {
                ExploredAt = DateTime.UtcNow;
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
