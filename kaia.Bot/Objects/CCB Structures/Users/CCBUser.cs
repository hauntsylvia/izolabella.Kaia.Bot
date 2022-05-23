using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures.Derivations;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class CCBUser : Unique
    {
        [JsonConstructor]
        private CCBUser(ulong Id, CCBUserSettings Settings) : base(Id)
        {
            this.Id = Id;
            this.Settings = Settings;
        }

        public new ulong Id { get; }

        [JsonProperty("Settings", Required = Required.Always)]
        public CCBUserSettings Settings { get; }

        public async Task<CCBUser> ChangeUserSettings(CCBUserSettings Settings)
        {
            CCBUser New = new(this.Id, Settings);
            await DataStores.UserStore.SaveAsync(New);
            return New;
        }
        public static async Task<CCBUser> GetOrCreateAsync(ulong Id, CCBUserSettings? Settings = null)
        {
            return (await DataStores.UserStore.ReadAsync<CCBUser>(Id)) ?? new(Id, Settings ?? new());
        }
    }
}
