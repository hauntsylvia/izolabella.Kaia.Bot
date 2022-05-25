using Kaia.Bot.Objects.CCB_Structures.Books.Properties;
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
        public CCBUserSettings(ulong? HighestCountEver = null, ulong? NumbersCounted = null, UserInventory? Inv = null, UserLibrary? Library = null)
        {
            this.HighestCountEver = HighestCountEver;
            this.NumbersCounted = NumbersCounted;
            this.Inventory = Inv ?? new(0.0);
            this.Library = Library ?? new();
        }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; set; }

        [JsonProperty("TotalNumbersCounted", Required = Required.Default)]
        public ulong? NumbersCounted { get; set; }

        [JsonProperty("Inventory", Required = Required.Default)]
        public UserInventory Inventory { get; set; }

        [JsonProperty("Library", Required = Required.Default)]
        public UserLibrary Library { get; set; }
    }
}
