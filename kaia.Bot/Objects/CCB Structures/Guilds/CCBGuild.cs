using Kaia.Bot.Objects.CCB_Structures.Derivations;
using Kaia.Bot.Objects.CCB_Structures.Guilds;
using Kaia.Bot.Objects.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CCBGuild : Unique
    {
        [JsonConstructor]
        private CCBGuild(ulong Id, CCBGuildSettings Settings) : base(Id)
        {
            this.Id = Id;
            this.Settings = Settings;
        }

        public new ulong Id { get; }

        [JsonProperty("Settings", Required = Required.Always)]
        public CCBGuildSettings Settings { get; }

        public async Task<CCBGuild> ChangeGuildSettings(CCBGuildSettings Settings)
        {
            CCBGuild New = new(this.Id, Settings);
            await DataStores.GuildStore.SaveAsync(New);
            return New;
        }

        public static async Task<CCBGuild> GetOrCreateAsync(ulong Id, CCBGuildSettings? Settings = null)
        {
            return (await DataStores.GuildStore.ReadAsync<CCBGuild>(Id)) ?? new(Id, Settings ?? new());
        }
    }
}
