﻿namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserInventory
    {
        [JsonConstructor]
        public UserInventory(double? Petals, DateTime? LastBookUpdate, params KaiaInventoryItem[]? Items)
        {
            this.Items = Items?.ToList() ?? new();
            this.LastBookUpdate = LastBookUpdate ?? DateTime.UtcNow;
            this.Petals = Petals ?? 10.0;
        }

        [JsonProperty("Items", Required = Required.Default)]
        public List<KaiaInventoryItem> Items { get; }

        [JsonProperty("LastBookUpdate", Required = Required.Default)]
        public DateTime LastBookUpdate { get; set; }

        private double petals;
        [JsonProperty("Currency", Required = Required.Default)]
        public double Petals { get => this.petals < 0 ? 0 : Math.Round(this.petals, 2); set => this.petals = value < 0 ? 0 : value; }

        public Task RemoveItemOfAsync(KaiaInventoryItem Item)
        {
            this.Items.RemoveAt(this.Items.FindIndex(A =>
            {
                return A.DisplayName == Item.DisplayName;
            }));
            return Task.CompletedTask;
        }
    }
}
