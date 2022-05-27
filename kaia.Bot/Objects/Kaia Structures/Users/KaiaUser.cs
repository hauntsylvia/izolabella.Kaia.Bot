using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using Kaia.Bot.Objects.CCB_Structures.Derivations;
using Kaia.Bot.Objects.CCB_Structures.Users;

namespace Kaia.Bot.Objects.CCB_Structures
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class KaiaUser : Unique
    {

        [JsonConstructor]
        public KaiaUser(ulong Id, KaiaUserSettings? Settings = null) : base(DataStores.UserStore, Id)
        {
            this.Id = Id;
            this.Settings = Settings ?? this.GetAsync<KaiaUser>().Result?.Settings ?? new(Id);
            this.Settings.LibraryProcessor = new(Id);

            List<KaiaBook> UserOwnedBooks = this.Settings.LibraryProcessor.GetUserBooksAsync().Result;
            double TotalToPay = 0.0;
            foreach (KaiaBook Book in UserOwnedBooks)
            {
                double CyclesMissed = (DateTime.UtcNow - this.Settings.Inventory.LastBookUpdate) / TimeSpans.BookTickRate;
                TotalToPay += Book.CurrentEarning * CyclesMissed;
            }
            this.Settings.Inventory.LastBookUpdate = DateTime.UtcNow;
            this.Settings.Inventory.Petals += TotalToPay;
        }

        public new ulong Id { get; }

        [JsonProperty("Settings", Required = Required.Always)]
        public KaiaUserSettings Settings { get; set; }
    }
}
