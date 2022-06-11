﻿using Kaia.Bot.Objects.KaiaStructures.Achievements.Properties;
using Kaia.Bot.Objects.KaiaStructures.Books.Properties;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace Kaia.Bot.Objects.KaiaStructures.Users
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaUserSettings
    {
        [JsonConstructor]
        public KaiaUserSettings(ulong U, ulong? HighestCountEver = null, ulong? NumbersCounted = null, UserInventory? Inv = null)
        {
            this.HighestCountEver = HighestCountEver ?? 0;
            this.NumbersCounted = NumbersCounted ?? 0;
            this.Inventory = Inv ?? new(0.0, DateTime.UtcNow);
            this.AchievementProcessor = new(U);
            this.LibraryProcessor = new(U);
        }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; set; }

        [JsonProperty("TotalNumbersCounted", Required = Required.Default)]
        public ulong? NumbersCounted { get; set; }

        [JsonProperty("Inventory", Required = Required.Default)]
        public UserInventory Inventory { get; set; }

        public UserAchievementRoom AchievementProcessor { get; set; }

        public UserLibrary LibraryProcessor { get; set; }
    }
}
