using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using Kaia.Bot.Objects.CCB_Structures.Derivations;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Properties;
using Kaia.Bot.Objects.CCB_Structures.Users;
using Kaia.Bot.Objects.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class CCBUser : Unique
    {

        [JsonConstructor]
        public CCBUser(ulong Id, CCBUserSettings? Settings = null) : base(DataStores.UserStore, Id)
        {
            this.Id = Id;
            this.Settings = Settings ?? this.GetAsync<CCBUser>().Result?.Settings ?? new(Id);
            this.Settings.LibraryProcessor = new(Id);

            List<KaiaBook> UserOwnedBooks = this.Settings.LibraryProcessor.GetUserBooksAsync().Result;
            double TotalToPay = 0.0;
            foreach (KaiaBook Book in UserOwnedBooks)
            {
                double CyclesMissed = ((DateTime.UtcNow - this.Settings.Inventory.LastBookUpdate) / TimeSpans.BookTickRate);
                TotalToPay += Book.CurrentEarning * CyclesMissed;
            }
            this.Settings.Inventory.LastBookUpdate = DateTime.UtcNow;
            this.Settings.Inventory.Petals += TotalToPay;
        }

        public new ulong Id { get; }

        [JsonProperty("Settings", Required = Required.Always)]
        public CCBUserSettings Settings { get; set; }
    }
}
