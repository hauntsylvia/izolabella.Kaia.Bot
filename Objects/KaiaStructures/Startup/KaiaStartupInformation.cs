using izolabella.Kaia.Bot.Objects.KaiaStructures.Derivations;
using izolabella.Storage.Objects.DataStores;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Startup
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class StartupProfile : Unique
    {
        [JsonConstructor]
        public StartupProfile(DataStore Belongs, string Token, string Alias, bool ProfileIsEnabled) : base(Belongs)
        {
            this.Token = Token;
            this.Alias = Alias;
            this.ProfileIsEnabled = ProfileIsEnabled;
        }

        [JsonProperty("Token", Required = Required.Always)]
        public string Token { get; }

        [JsonProperty("DisplayName", Required = Required.DisallowNull)]
        public string Alias { get; }

        [JsonProperty("ProfileIsEnabled", Required = Required.DisallowNull)]
        public bool ProfileIsEnabled { get; }
    }
}
