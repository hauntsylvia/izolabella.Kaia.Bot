using izolabella.Kaia.Bot.Objects.KaiaStructures.Derivations;
using izolabella.Storage.Objects.DataStores;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Startup
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    [method: JsonConstructor]
    public class StartupProfile(DataStore Belongs, string Token, string Alias, bool ProfileIsEnabled) : Unique(Belongs)
    {
        [JsonProperty(nameof(Token), Required = Required.Always)]
        public string Token { get; } = Token;

        [JsonProperty("DisplayName", Required = Required.DisallowNull)]
        public string Alias { get; } = Alias;

        [JsonProperty(nameof(ProfileIsEnabled), Required = Required.DisallowNull)]
        public bool ProfileIsEnabled { get; } = ProfileIsEnabled;
    }
}