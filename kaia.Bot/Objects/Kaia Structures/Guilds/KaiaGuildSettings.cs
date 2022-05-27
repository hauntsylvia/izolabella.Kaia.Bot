using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Guilds
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaGuildSettings
    {
        [JsonConstructor]
        public KaiaGuildSettings(ulong? CountingChannelId = null, ulong? LastSuccessfulNumber = null, ulong? LastUserWhoCounted = null, ulong? HighestCountEver = null, IReadOnlyDictionary<string, GuildPermission[]>? OverrideCommandPermissionsConstraint = null)
        {
            this.CountingChannelId = CountingChannelId;
            this.LastSuccessfulNumber = LastSuccessfulNumber;
            this.LastUserWhoCounted = LastUserWhoCounted ?? this.LastUserWhoCounted;
            this.HighestCountEver = HighestCountEver ?? this.HighestCountEver;
            this.OverrideCommandPermissionsConstraint = OverrideCommandPermissionsConstraint ?? this.OverrideCommandPermissionsConstraint;
        }

        [JsonProperty("CountingChannelId", Required = Required.AllowNull)]
        public ulong? CountingChannelId { get; set; }

        [JsonProperty("LastSuccessfulNumber", Required = Required.AllowNull)]
        public ulong? LastSuccessfulNumber { get; set; }

        [JsonProperty("LastUserWhoCounted", Required = Required.AllowNull)]
        public ulong? LastUserWhoCounted { get; set; }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; set; }

        [JsonProperty("OverrideCommandPermissionsConstraint", Required = Required.Default)]
        public IReadOnlyDictionary<string, GuildPermission[]> OverrideCommandPermissionsConstraint { get; set; } = new Dictionary<string, GuildPermission[]>();

        [JsonProperty("OverrideCommandRolesConstraint", Required = Required.Default)]
        public IReadOnlyDictionary<string, ulong[]> OverrideCommandRolesConstraint { get; set; } = new Dictionary<string, ulong[]>();
    }
}
