using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

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
                             KaiaEmote DisplayEmoteName,
                             KaiaItemReturnContext? OnInteract = null,
                             ulong? Id = null)
    {
        this.DisplayName = DisplayName;
        this.Description = Description;
        this.MarketCost = MarketCost;
        this.CanInteractWithDirectly = CanInteractWithDirectly;
        this.KaiaDisplaysThisOnTheStore = KaiaDisplaysThisOnTheStore;
        this.UsersCanSellThis = UsersCanSellThis;
        this.DisplayEmote = DisplayEmoteName;
        this.OnInteract = OnInteract;
        this.Id = Id ?? IdGenerator.CreateNewId();
    }

    [JsonProperty("DisplayName")]
    public string DisplayName { get; }

    /// <summary>
    /// $"[{ExplorationStrings.Economy.CurrencyEmote} {ItemChunk.Key.MarketCost}] {ItemChunk.Key.DisplayName}"
    /// </summary>
    public string DisplayString => $"[{Strings.Economy.CurrencyEmote} `{this.MarketCost}`] {this.DisplayName}";

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
    public KaiaEmote DisplayEmote { get; set; }

    [JsonProperty("OnInteract")]
    public KaiaItemReturnContext? OnInteract { get; protected set; }

    [JsonProperty("Id")]
    public ulong Id { get; set; }

    [JsonProperty("ReceivedAt")]
    public DateTime? ReceivedAt { get; set; }

    public virtual Task OnKaiaStoreRefresh()
    {
        return Task.CompletedTask;
    }
}
