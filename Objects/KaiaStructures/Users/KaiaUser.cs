using Kaia.Bot.Objects.KaiaStructures.Achievements.Properties;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Properties;
using Kaia.Bot.Objects.KaiaStructures.Derivations;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Implementations.Blessings;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace Kaia.Bot.Objects.KaiaStructures.Users
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class KaiaUser : Unique
    {
        [JsonConstructor]
        public KaiaUser(ulong Id, KaiaUserSettings? Settings = null) : base(DataStores.UserStore, Id)
        {
            if(Id > 0)
            {
                this.Id = Id;
                this.LibraryProcessor = new(Id);
                this.AchievementProcessor = new(Id);
                this.LocationProcessor = new(Id);
                this.SpellsProcessor = new(Id);
                this.RelationshipsProcessor = new(Id);
                KaiaUserSettings? S = Settings ?? this.GetAsync<KaiaUser>().Result?.Settings;
                this.Exists = S != null;
                this.Settings = S ?? new(Id);

                List<KaiaBook> UserOwnedBooks = this.LibraryProcessor.GetUserBooksAsync().Result;
                double TotalToPay = 0.0;
                foreach (KaiaBook Book in UserOwnedBooks)
                {
                    double CyclesMissed = (DateTime.UtcNow - this.Settings.Inventory.LastBookUpdate) / TimeSpans.BookTickRate;
                    TotalToPay += Book.CurrentEarning * CyclesMissed * this.TotalMultiplierOnBooks;
                }

                this.Settings.Inventory.LastBookUpdate = DateTime.UtcNow;
                this.Settings.Inventory.Petals += TotalToPay;
            }
            else
            {
                throw new ArgumentException(message: "The id of the user can not be 0.", paramName: nameof(Id));
            }
        }

        [JsonProperty("Id", Required = Required.DisallowNull)]
        public new ulong Id { get; }

        [JsonProperty("Settings", Required = Required.Always)]
        public KaiaUserSettings Settings { get; set; }

        public double TotalMultiplierOnBooks => 1 + Math.Round(this.SpellsProcessor.GetActiveSpellsAsync().Result
                                                                         .Count(A => A.GetType() == typeof(BookIncomeIncrease))
                                                                         * BookIncomeIncrease.MultiplyBy, 2);

        [JsonIgnore]
        public UserAchievementRoom AchievementProcessor { get; set; }

        [JsonIgnore]
        public UserLibrary LibraryProcessor { get; set; }

        [JsonIgnore]
        public UserLocationRoom LocationProcessor { get; set; }

        [JsonIgnore]
        public SpellsProcessor SpellsProcessor { get; set; }

        [JsonIgnore]
        public RelationshipsProcessor RelationshipsProcessor { get; set; }

        [JsonProperty("Exists", Required = Required.Default)]
        public bool Exists { get; }
    }
}
