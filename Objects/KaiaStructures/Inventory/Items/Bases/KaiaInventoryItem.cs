using izolabella.Util;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaInventoryItem
    {
        [JsonConstructor]
        public KaiaInventoryItem(string DisplayName,
                                 string Description,
                                 double MarketCost,
                                 bool CanInteractWithDirectly,
                                 bool KaiaDisplaysThisOnTheStore,
                                 bool UsersCanSellThis,
                                 KaiaItemEmote DisplayEmoteName,
                                 ulong? Id = null)
        {
            this.DisplayName = DisplayName;
            this.Description = Description;
            this.MarketCost = MarketCost;
            this.CanInteractWithDirectly = CanInteractWithDirectly;
            this.KaiaDisplaysThisOnTheStore = KaiaDisplaysThisOnTheStore;
            this.UsersCanSellThis = UsersCanSellThis;
            this.DisplayEmote = DisplayEmoteName;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; }

        [JsonProperty("Description")]
        public string Description { get; }

        [JsonProperty("Cost")]
        public double MarketCost { get; }

        [JsonProperty("CanInteractWithDirectly")]
        public bool CanInteractWithDirectly { get; }

        [JsonProperty("KaiaDisplaysThisOnTheStore")]
        public bool KaiaDisplaysThisOnTheStore { get; }

        [JsonProperty("UsersCanSellThis")]
        public bool UsersCanSellThis { get; }

        [JsonProperty("DisplayEmote")]
        public KaiaItemEmote DisplayEmote { get; set; }

        [JsonProperty("Id")]
        public ulong Id { get; set; }

        [JsonProperty("ReceivedAt")]
        public DateTime? ReceivedAt { get; set; }

        public virtual Task UserInteractAsync(CommandContext Context, KaiaUser User)
        {
            return Task.CompletedTask;
        }
    }
}
