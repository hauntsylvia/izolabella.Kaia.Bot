using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Users
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaUserSettings
    {
        [JsonConstructor]
        public KaiaUserSettings(ulong U,
                                ulong? HighestCountEver = null,
                                ulong? NumbersCounted = null,
                                UserInventory? Inv = null)
        {
            this.HighestCountEver = HighestCountEver ?? 0;
            this.NumbersCounted = NumbersCounted ?? 0;
            this.Inventory = Inv ?? new(30, DateTime.UtcNow);
            this.U = U;
        }

        [JsonProperty("ParentId", Required = Required.DisallowNull)]
        public ulong U { get; }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; set; }

        [JsonProperty("TotalNumbersCounted", Required = Required.Default)]
        public ulong? NumbersCounted { get; set; }

        [JsonProperty("Inventory", Required = Required.Default)]
        public UserInventory Inventory { get; set; }
    }
}
