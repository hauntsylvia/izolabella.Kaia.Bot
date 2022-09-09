using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Storage.Objects.Structures;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;

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
        this.displayRewards = DisplayRewards;
        this.Id = SuperSecretSelfId;
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

    public bool DisplayRewards => this.displayRewards && this.Events.Any();

    public IEnumerable<KaiaLocationEvent> Events { get; }

    public TimeSpan AvailableAt { get; }

    public TimeSpan AvailableTo { get; }

    public DateTime? AvailableUntil { get; set; }

    public TimeSpan MinimumTimeBetweenExplorations { get; }

    public KaiaEmote Emote { get; }

    public DateTime? ExploredAt { get; private set; }

    public DateTime? MustWaitUntil => this.ExploredAt.HasValue ? this.ExploredAt.Value.Add(this.MinimumTimeBetweenExplorations) : null;

    public Uri? CoverUrl { get; }

    public Uri? CoverUrlCredit { get; }

    public bool Overnight => this.AvailableAt > this.AvailableTo;

    public DateTime InnerClock { get; } = DateTime.UtcNow;

    public DateTime Start => this.InnerClock.Date.Add(this.AvailableAt);

    public DateTime End => this.Overnight ? this.InnerClock.Date.AddDays(1).Add(this.AvailableTo) : this.InnerClock.Date.Add(this.AvailableTo);

    [JsonProperty("SuperSecretSelfId")]
    public ulong Id { get; }

    public KaiaLocationExplorationStatus Status =>
        this.Overnight && this.InnerClock.TimeOfDay > this.AvailableTo && this.InnerClock.TimeOfDay < this.AvailableAt ? KaiaLocationExplorationStatus.IncorrectLocationTime :
        !this.Overnight && (this.InnerClock < this.Start || this.InnerClock > this.End) ? KaiaLocationExplorationStatus.IncorrectLocationTime :
        this.AvailableUntil.HasValue && this.InnerClock < this.AvailableUntil.Value ? KaiaLocationExplorationStatus.LocationUnavailable :
        this.MustWaitUntil.HasValue && this.MustWaitUntil.Value > this.InnerClock ? KaiaLocationExplorationStatus.Timeout :
        KaiaLocationExplorationStatus.Successful;

    public KaiaLocationExplorationStatus StatusNoTimeout => this.Status == KaiaLocationExplorationStatus.Timeout ? KaiaLocationExplorationStatus.Successful : this.Status;

    private KaiaLocationEvent GetEvent()
    {
        double TotalWeight = this.Events.Sum(A => A.Weight);
        double Random = new Random().Next(0, (int)TotalWeight) + TotalWeight - (int)TotalWeight;
        foreach (KaiaLocationEvent Event in this.Events)
        {
            Random -= Event.Weight;
            if (Random <= 0)
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
