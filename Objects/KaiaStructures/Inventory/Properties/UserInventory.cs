using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

[JsonObject(MemberSerialization.OptIn)]
public class UserInventory
{
    [JsonConstructor]
    public UserInventory(double? Petals, DateTime? LastBookUpdate, params KaiaInventoryItem[]? Items)
    {
        this.items = Items?.ToList() ?? new();
        this.LastBookUpdate = LastBookUpdate ?? DateTime.UtcNow;
        this.Petals = Petals ?? 10.0;
    }

    [JsonProperty("Items", Required = Required.Default)]
    private readonly List<KaiaInventoryItem> items = new();

    public IReadOnlyCollection<KaiaInventoryItem> Items => this.items;

    [JsonProperty("LastBookUpdate", Required = Required.Default)]
    public DateTime LastBookUpdate { get; set; }

    private double petals;
    [JsonProperty("Currency", Required = Required.Default)]
    public double Petals { get => this.petals < 0 ? 0 : Math.Round(this.petals, 2); set => this.petals = value < 0 ? 0 : value; }

    /// <summary>
    /// Removes an item from the user's inventory with the same id.
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public Task<bool> RemoveItemOfIdAsync(KaiaInventoryItem Item)
    {
        int I = this.items.FindIndex(A => A.Id == Item.Id);
        if (I >= 0)
        {
            this.items.RemoveAt(I);
            return Task.FromResult(true);
        }
        else
        {
            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// Removes an item from the user's inventory with the same display name.
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public async Task RemoveItemOfNameAsync(KaiaUser Parent, KaiaInventoryItem Item)
    {
        int I = this.items.FindIndex(A => A.DisplayName == Item.DisplayName);
        if (I >= 0)
        {
            this.items.RemoveAt(I);
        }
        await Parent.SaveAsync();
    }

    /// <summary>
    /// Gets an item from the user's inventory by a specific display name.
    /// </summary>
    /// <returns></returns>
    public Task<KaiaInventoryItem?> GetItemOfDisplayName(KaiaInventoryItem? Item)
    {
        return Task.FromResult(this.Items.FirstOrDefault(I => I.DisplayName == Item?.DisplayName));
    }

    /// <summary>
    /// Gets all items from the user's inventory by a specific display name.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<KaiaInventoryItem>> GetItemsOfDisplayNameFromItem(KaiaInventoryItem? Item)
    {
        return Task.FromResult(this.Items.Where(I => I.DisplayName == Item?.DisplayName));
    }

    /// <summary>
    /// Gets all items from the user's inventory by a specific display name.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<KaiaInventoryItem>> GetItemsOfDisplayName(string DisplayName)
    {
        return Task.FromResult(this.Items.Where(I => I.DisplayName == DisplayName));
    }

    /// <summary>
    /// Retrieves an item based on a given id.
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public Task<KaiaInventoryItem?> GetItemOfId(ulong ItemId)
    {
        return Task.FromResult(this.Items.FirstOrDefault(I => I.Id == ItemId));
    }

    /// <summary>
    /// Returns true when the user has an item of a type.
    /// </summary>
    /// <typeparam name="TItemType"></typeparam>
    /// <returns></returns>
    public Task<bool> ItemOfTypeExists<TItemType>() where TItemType : KaiaInventoryItem
    {
        return Task.FromResult(this.Items.Any(I => I.GetType() == typeof(TItemType)));
    }

    /// <summary>
    /// Returns true when the user has an item of a display name.
    /// </summary>
    /// <returns></returns>
    public Task<bool> ItemOfDisplayNameExists(KaiaInventoryItem? Item)
    {
        return Task.FromResult(this.Items.Any(I => I.DisplayName == Item?.DisplayName));
    }

    /// <summary>
    /// Adds items to the user's inventory before saving the user.
    /// </summary>
    /// <param name="Parent">The user whom of which should be saved.</param>
    /// <param name="Items">The items to add.</param>
    /// <returns></returns>
    public async Task AddItemsToInventoryAndSaveAsync(KaiaUser Parent, params KaiaInventoryItem[] Items)
    {
        foreach (KaiaInventoryItem Item in Items)
        {
            await Item.OnKaiaStoreRefresh();
            Item.ReceivedAt = DateTime.UtcNow;
        }
        this.items.AddRange(Items);
        await Parent.SaveAsync();
    }
}
