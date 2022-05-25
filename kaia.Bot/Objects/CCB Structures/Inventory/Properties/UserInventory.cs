using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Inventory.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserInventory
    {
        [JsonConstructor]
        public UserInventory(double? Petals, DateTime? LastBookUpdate, params ICCBInventoryItem[]? Items)
        {
            this.Items = Items?.ToList() ?? new();
            this.LastBookUpdate = LastBookUpdate ?? DateTime.UtcNow;
            this.Petals = Petals ?? 10.0;
        }

        [JsonProperty("Items", Required = Required.Default)]
        public List<ICCBInventoryItem> Items { get; }

        [JsonProperty("LastBookUpdate", Required = Required.Default)]
        public DateTime LastBookUpdate { get; set; }

        private double petals;
        [JsonProperty("Currency", Required = Required.Default)]
        public double Petals { get => Math.Round(this.petals, 2); set => this.petals = value; }
    }
}
