using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures.Users
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CCBUserSettings
    {
        [JsonConstructor]
        public CCBUserSettings(ulong? HighestCountEver = null, ulong? NumbersCounted = null)
        {
            this.HighestCountEver = HighestCountEver;
            this.NumbersCounted = NumbersCounted;
        }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; }

        [JsonProperty("TotalNumbersCounted", Required = Required.Default)]
        public ulong? NumbersCounted { get; }
    }
}
