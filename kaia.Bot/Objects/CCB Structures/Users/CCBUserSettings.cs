using Kaia.Bot.Objects.CCB_Structures.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Users
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CCBUserSettings
    {
        [JsonConstructor]
        public CCBUserSettings(ulong? HighestCountEver = null, ulong? NumbersCounted = null, CCBUserInventory? Inv = null)
        {
            this.HighestCountEver = HighestCountEver;
            this.NumbersCounted = NumbersCounted;
            this.Inventory = Inv ?? new(0.0m, 0.0m);
        }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; set; }

        [JsonProperty("TotalNumbersCounted", Required = Required.Default)]
        public ulong? NumbersCounted { get; set; }

        [JsonProperty("Inventory", Required = Required.Default)]
        public CCBUserInventory Inventory { get; set; }
    }
}
