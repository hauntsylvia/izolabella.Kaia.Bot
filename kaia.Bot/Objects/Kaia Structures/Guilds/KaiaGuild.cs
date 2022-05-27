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
    public class KaiaGuild : Unique
    {
        [JsonConstructor]
        public KaiaGuild(ulong Id, KaiaGuildSettings? Settings = null) : base(DataStores.GuildStore, Id)
        {
            this.Id = Id;
            this.settings = Settings ?? this.GetAsync<KaiaGuild>().Result?.Settings ?? new();
        }

        public new ulong Id { get; }

        private KaiaGuildSettings settings;
        [JsonProperty("Settings", Required = Required.Always)]
        public KaiaGuildSettings Settings
        {
            get => this.settings;
            set
            {
                this.settings = value;
                this.SaveAsync().GetAwaiter().GetResult();
            }
        }
    }
}
