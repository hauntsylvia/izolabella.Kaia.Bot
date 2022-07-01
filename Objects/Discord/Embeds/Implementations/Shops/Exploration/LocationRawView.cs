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

        public KaiaLocation Location { get; }

        protected override Task ClientRefreshAsync()
        {
            if(this.Location.CoverUrl != null)
            {
                this.WithImage(this.Location.CoverUrl);
            }
            this.WithField("description", $"```{this.Location.Description}```");
            if(this.Location.DisplayRewards)
            {
                List<string> Display = new();
                int TotalWeight = this.Location.Events.Sum(KV => KV.Weight);
                foreach(KaiaLocationEvent A in this.Location.Events.OrderBy(KV => KV.Weight))
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
            return Task.CompletedTask;
        }
    }
}
