using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationExplorationResult : KaiaPathEmbed
    {
        public LocationExplorationResult(KaiaLocation Location, KaiaLocationEvent Event) : base(Strings.EmbedStrings.FakePaths.Outside, Location.Name)
        {
            if(Event.Status == KaiaStructures.Exploration.Locations.Enums.KaiaLocationExplorationStatus.Successful)
            {
                this.WithField("?", $"```{Event.Message}```");
                if (Event.RewardResult.Petals is > 0 or < 0)
                {
                    string F = $"{Strings.Economy.CurrencyEmote} `{Event.RewardResult.Petals}`";
                    this.WithField($"{Strings.Economy.CurrencyName}", $"{(Event.RewardResult.Petals > 0 ? $"u've found {F}" : $"u r down {F}")}");
                }
                if (Event.RewardResult.Items.Length > 0)
                {
                    List<string> Display = new();
                    foreach (KaiaInventoryItem Item in Event.RewardResult.Items)
                    {
                        Display.Add($"[worth {Strings.Economy.CurrencyEmote} `{Item.MarketCost}`] {Item.DisplayName} {Item.DisplayEmote}");
                    }
                    this.WithListWrittenToField("items found", Display, ",\n");
                }
            }
            else
            {
                this.WithField("nothing!", "looks like something went wrong");
            }
        }
    }
}
