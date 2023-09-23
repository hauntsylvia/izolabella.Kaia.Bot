using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [method: JsonConstructor]
    public class KaiaInventoryItem(string DisplayName,
                             string Description,
                             double MarketCost,
                             bool CanInteractWithDirectly,
                             bool KaiaDisplaysThisOnTheStore,
                             bool UsersCanSellThis,
                             KaiaEmote DisplayEmoteName,
                             KaiaItemReturnContext? OnInteract = null,
                             ulong? Id = null)
    {
        [JsonProperty(nameof(DisplayName))]
        public string DisplayName { get; } = DisplayName;

        /// <summary>
        /// $"[{ExplorationStrings.Economy.CurrencyEmote} {ItemChunk.Key.MarketCost}] {ItemChunk.Key.DisplayName}"
        /// </summary>
        public string DisplayString => $"[{Strings.Economy.CurrencyEmote} `{this.MarketCost}`] {this.DisplayName}";

        [JsonProperty(nameof(Description))]
        public string Description { get; } = Description;

        [JsonProperty("Cost")]
        public double MarketCost { get; } = MarketCost;

        [JsonProperty(nameof(CanInteractWithDirectly))]
        public bool CanInteractWithDirectly { get; } = CanInteractWithDirectly;

        [JsonProperty(nameof(KaiaDisplaysThisOnTheStore))]
        public bool KaiaDisplaysThisOnTheStore { get; } = KaiaDisplaysThisOnTheStore;

        [JsonProperty(nameof(UsersCanSellThis))]
        public bool UsersCanSellThis { get; } = UsersCanSellThis;

        [JsonProperty(nameof(DisplayEmote))]
        public KaiaEmote DisplayEmote { get; set; } = DisplayEmoteName;

        [JsonProperty(nameof(OnInteract))]
        public KaiaItemReturnContext? OnInteract { get; protected set; } = OnInteract;

        [JsonProperty(nameof(Id))]
        public ulong Id { get; set; } = Id ?? IdGenerator.CreateNewId();

        [JsonProperty(nameof(ReceivedAt))]
        public DateTime? ReceivedAt { get; set; }

        public virtual Task OnKaiaStoreRefresh()
        {
            return Task.CompletedTask;
        }
    }
}