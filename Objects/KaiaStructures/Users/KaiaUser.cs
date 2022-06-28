using Kaia.Bot.Objects.KaiaStructures.Achievements.Properties;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Properties;
using Kaia.Bot.Objects.KaiaStructures.Derivations;

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
                this.Settings = Settings ?? this.GetAsync<KaiaUser>().Result?.Settings ?? new(Id);
                this.LibraryProcessor = new(Id);
                this.AchievementProcessor = new(Id);

                List<KaiaBook> UserOwnedBooks = this.LibraryProcessor.GetUserBooksAsync().Result;
                double TotalToPay = 0.0;
                foreach (KaiaBook Book in UserOwnedBooks)
                {
                    double CyclesMissed = (DateTime.UtcNow - this.Settings.Inventory.LastBookUpdate) / TimeSpans.BookTickRate;
                    TotalToPay += Book.CurrentEarning * CyclesMissed;
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

        public UserAchievementRoom AchievementProcessor { get; set; }

        public UserLibrary LibraryProcessor { get; set; }
    }
}
