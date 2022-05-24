using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.CCB_Structures.Derivations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Startup
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class CCBStartupInformation : Unique
    {
        [JsonConstructor]
        public CCBStartupInformation(DataStore Belongs, string Token) : base(Belongs)
        {
            this.Token = Token;
        }

        [JsonProperty("Token", Required = Required.Always)]
        public string Token { get; }
    }
}
