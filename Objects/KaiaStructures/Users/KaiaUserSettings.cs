﻿using Kaia.Bot.Objects.KaiaStructures.Achievements;
using Kaia.Bot.Objects.KaiaStructures.Books.Properties;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace Kaia.Bot.Objects.KaiaStructures.Users
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaUserSettings
    {
        [JsonConstructor]
        public KaiaUserSettings(ulong U, ulong? HighestCountEver = null, ulong? NumbersCounted = null, List<KaiaAchievement>? Achievements = null, UserInventory? Inv = null)
        {
            this.HighestCountEver = HighestCountEver ?? 0;
            this.NumbersCounted = NumbersCounted ?? 0;
            this.Achievements = Achievements ?? new();
            this.Inventory = Inv ?? new(0.0, DateTime.UtcNow);
            this.LibraryProcessor = new(U);
        }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; set; }

        [JsonProperty("TotalNumbersCounted", Required = Required.Default)]
        public ulong? NumbersCounted { get; set; }

        [JsonProperty("Achievements", Required = Required.Default)]
        public List<KaiaAchievement> Achievements { get; set; }

        [JsonProperty("Inventory", Required = Required.Default)]
        public UserInventory Inventory { get; set; }

        public UserLibrary LibraryProcessor { get; set; }
    }
}
