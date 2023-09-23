using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Users
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [method: JsonConstructor]
    public class KaiaUserSettings(ulong U,
                            ulong? HighestCountEver = null,
                            ulong? NumbersCounted = null,
                            UserInventory? Inv = null)
    {
        [JsonProperty("ParentId", Required = Required.DisallowNull)]
        public ulong U { get; } = U;

        [JsonProperty(nameof(HighestCountEver), Required = Required.Default)]
        public ulong? HighestCountEver { get; set; } = HighestCountEver ?? 0;

        [JsonProperty("TotalNumbersCounted", Required = Required.Default)]
        public ulong? NumbersCounted { get; set; } = NumbersCounted ?? 0;

        [JsonProperty(nameof(Inventory), Required = Required.Default)]
        public UserInventory Inventory { get; set; } = Inv ?? new(30, DateTime.UtcNow);
    }
}