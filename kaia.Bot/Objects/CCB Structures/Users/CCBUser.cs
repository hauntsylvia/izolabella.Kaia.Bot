using Kaia.Bot.Objects.CCB_Structures.Derivations;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Properties;
using Kaia.Bot.Objects.CCB_Structures.Users;
using Kaia.Bot.Objects.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class CCBUser : Unique
    {
        [JsonConstructor]
        public CCBUser(ulong Id, CCBUserCountingInfo? CountingInfo = null, CCBUserInventory? Inventory = null) : base(DataStores.UserStore, Id)
        {
            this.Id = Id;
            this.countingInfo = CountingInfo ?? this.GetAsync<CCBUser>().Result?.CountingInfo ?? new();
            this.inventory = Inventory ?? this.GetAsync<CCBUser>().Result?.Inventory ?? new();
        }

        public new ulong Id { get; }

        private CCBUserCountingInfo countingInfo;
        [JsonProperty("Settings", Required = Required.Always)]
        public CCBUserCountingInfo CountingInfo
        {
            get => this.countingInfo ?? new();
            set
            {
                this.countingInfo = value;
                this.SaveAsync().GetAwaiter().GetResult();
            }
        }

        private CCBUserInventory? inventory;
        [JsonProperty("Inventory", Required = Required.DisallowNull)]
        public CCBUserInventory Inventory
        {
            get => this.inventory ?? this.GetAsync<CCBUser>().Result?.inventory ?? new();
            set
            {
                this.inventory = value;
                this.SaveAsync().GetAwaiter().GetResult();
            }
        }
    }
}
