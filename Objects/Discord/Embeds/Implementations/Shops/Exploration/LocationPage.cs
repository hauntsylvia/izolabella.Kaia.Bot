using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationPage : KaiaPathEmbedRefreshable
    {
        public LocationPage(IEnumerable<KaiaLocation> Locations, KaiaUser U) : base(Strings.EmbedStrings.FakePaths.Outside)
        {
            this.Locations = Locations;
            this.U = U;
        }

        public IEnumerable<KaiaLocation> Locations { get; }

        public KaiaUser U { get; }

        protected override Task ClientRefreshAsync()
        {
            foreach (KaiaLocation Location in this.Locations)
            {
                List<string> Display = new()
                {
                    Location.Status == KaiaLocationExplorationStatus.LocationUnavailable ?
                        $"this location is not available for exploration" :
                        Location.Status == KaiaLocationExplorationStatus.Timeout ?
                            $"u must wait before exploring this location again" :
                            Location.ShortDescription
                };
                if (Location.AvailableUntil.HasValue && Location.StatusNoTimeout == KaiaLocationExplorationStatus.IncorrectLocationTime)
                {
                    Display.Add($"`{Math.Round((Location.AvailableUntil.Value - DateTime.UtcNow).TotalDays, 0)}` days left");
                }
                this.WithListWrittenToField(Location.Name, Display, "\n");
            }
            return Task.CompletedTask;
        }
    }
}
