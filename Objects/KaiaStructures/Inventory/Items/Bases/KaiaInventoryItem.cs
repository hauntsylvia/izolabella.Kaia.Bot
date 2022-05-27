using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties;
using Kaia.Bot.Objects.KaiaStructures.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaInventoryItem
    {
        [JsonConstructor]
        public KaiaInventoryItem(string DisplayName,
                                 string Description,
                                 double Cost,
                                 bool CanInteractWithDirectly,
                                 KaiaItemEmote DisplayEmoteName)
        {
            this.DisplayName = DisplayName;
            this.Description = Description;
            this.Cost = Cost;
            this.CanInteractWithDirectly = CanInteractWithDirectly;
            this.DisplayEmote = DisplayEmoteName;
        }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; }

        [JsonProperty("Description")]
        public string Description { get; }

        [JsonProperty("Cost")]
        public double Cost { get; }

        [JsonProperty("CanInteractWithDirectly")]
        public bool CanInteractWithDirectly { get; }

        [JsonProperty("DisplayEmote")]
        public KaiaItemEmote DisplayEmote { get; set; }

        [JsonProperty("ReceivedAt")]
        public DateTime? ReceivedAt { get; private set; }

        public Task UserBoughtAsync(KaiaUser User)
        {
            User.Settings.Inventory.Items.Add(this);
            User.Settings.Inventory.Petals -= this.Cost;
            this.ReceivedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public virtual Task UserInteractAsync(CommandContext Context, KaiaUser User)
        {
            return Task.CompletedTask;
        }
    }
}
