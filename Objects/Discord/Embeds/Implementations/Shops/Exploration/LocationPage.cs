using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationPage(IEnumerable<KaiaLocation> Locations, KaiaUser U) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Outside)
    {
        public IEnumerable<KaiaLocation> Locations { get; } = Locations;

        public KaiaUser U { get; } = U;

        protected override Task ClientRefreshAsync()
        {
            foreach (KaiaLocation Location in this.Locations)
            {
                List<string> Display = new()
            {
                $"```{(Location.StatusNoTimeout == KaiaLocationExplorationStatus.LocationUnavailable ?
                    $"this location is not available for exploration anymore" :
                    Location.Status == KaiaLocationExplorationStatus.Timeout ?
                        $"u must wait before exploring this location again" :
                        Location.ShortDescription)}```"
            };
                if (Location.AvailableUntil.HasValue && Location.StatusNoTimeout != KaiaLocationExplorationStatus.LocationUnavailable)
                {
                    Display.Add($"`{Math.Round((Location.AvailableUntil.Value - DateTime.UtcNow).TotalDays, 0)}` days left");
                }
                Display.Add(Strings.EmbedStrings.Empty);
                this.WithListWrittenToField($"{Location.Name}", Display, "\n");
            }
            return Task.CompletedTask;
        }
    }
}