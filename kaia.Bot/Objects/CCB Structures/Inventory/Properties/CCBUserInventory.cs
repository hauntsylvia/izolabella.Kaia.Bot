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
        public CCBUserInventory(params ICCBInventoryItem[]? Items)
        {
            this.Items = Items?.ToList() ?? new();
        }

        [JsonProperty("Items", Required = Required.Default)]
        public List<ICCBInventoryItem> Items { get; }
    }
}
