using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.CCB_Structures.Derivations;

namespace Kaia.Bot.Objects.CCB_Structures.Startup
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class KaiaStartupInformation : Unique
    {
        [JsonConstructor]
        public KaiaStartupInformation(DataStore Belongs, string Token) : base(Belongs)
        {
            this.Token = Token;
        }

        [JsonProperty("Token", Required = Required.Always)]
        public string Token { get; }
    }
}
