using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationRawView : KaiaPathEmbedRefreshable
    {
        public LocationRawView(CommandContext Context, KaiaLocation Location) : base(Strings.EmbedStrings.FakePaths.Outside, Location.Name)
        {
            this.Context = Context;
            this.Location = Location;
        }

        public CommandContext Context { get; }

        public KaiaLocation Location { get; set; }

        protected override async Task ClientRefreshAsync()
        {
            this.Location = (await new KaiaUser(this.Context.UserContext.User.Id).LocationProcessor.GetUserLocationsExploredAsync()).FirstOrDefault(A => A.Id == this.Location.Id) ?? this.Location;
            if (this.Location.CoverUrl != null)
            {
                this.WithImage(this.Location.CoverUrl);
            }
            this.WithField("description", $"```{this.Location.Description}```");
            if(this.Location.MustWaitUntil != null && this.Location.Status == KaiaStructures.Exploration.Locations.Enums.KaiaLocationExplorationStatus.Timeout)
            {
                this.WithField("timeout!", $"please wait for `{(this.Location.MustWaitUntil - DateTime.UtcNow).Value.Hours}` hours before exploring this place again!");
            }
            if(this.Location.DisplayRewards)
            {
                List<string> Display = new();
                int TotalWeight = this.Location.Events.Sum(KV => KV.Weight);
                foreach(KaiaLocationEvent A in this.Location.Events.OrderBy(KV => KV.Weight).Take(3))
                {
                    string SubDisplay = string.Empty;
                    double Chance = (double)((double)((double)A.Weight / (double)TotalWeight) * 100f);
                    foreach (KaiaInventoryItem Item in A.RewardResult.Items)
                    {
                        SubDisplay += $"→ {Item.DisplayEmote} {Item.DisplayName} [{Math.Round(Chance, 2)}%]";
                        if(Item != A.RewardResult.Items.Last())
                        {
                            SubDisplay += "\n";
                        }
                    }
                    Display.Add(SubDisplay);
                }
                this.WithListWrittenToField("rarest potential finds", Display, "\n");
            }
        }
    }
}
