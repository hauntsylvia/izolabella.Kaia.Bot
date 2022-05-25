using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Inventory.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CCBUserInventory
    {
        [JsonConstructor]
        public CCBUserInventory(decimal? Petals, decimal? Ego, params ICCBInventoryItem[]? Items)
        {
            this.Items = Items?.ToList() ?? new();
            this.Petals = Petals ?? 10.0m;
            this.Ego = Ego ?? 0.0m;
        }

        [JsonProperty("Items", Required = Required.Default)]
        public List<ICCBInventoryItem> Items { get; }

        private decimal petals;
        [JsonProperty("Currency", Required = Required.Default)]
        public decimal Petals { get => decimal.Round(this.petals, 2); set => this.petals = value; }

        private decimal ego;
        [JsonProperty("Ego", Required = Required.Default)]
        public decimal Ego { get => decimal.Round(this.ego, 2); set => this.ego = value; }
    }
}
