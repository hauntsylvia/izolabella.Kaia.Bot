using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures.Guilds
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CCBGuildSettings
    {
        [JsonConstructor]
        public CCBGuildSettings(ulong? CountingChannelId = null, ulong? LastSuccessfulNumber = null, ulong? LastUserWhoCounted = null, ulong? HighestCountEver = null, IReadOnlyDictionary<string, GuildPermission[]>? OverrideCommandPermissionsConstraint = null)
        {
            this.CountingChannelId = CountingChannelId;
            this.LastSuccessfulNumber = LastSuccessfulNumber;
            this.LastUserWhoCounted = LastUserWhoCounted ?? this.LastUserWhoCounted;
            this.HighestCountEver = HighestCountEver ?? this.HighestCountEver;
            this.OverrideCommandPermissionsConstraint = OverrideCommandPermissionsConstraint ?? this.OverrideCommandPermissionsConstraint;
        }

        [JsonProperty("CountingChannelId", Required = Required.AllowNull)]
        public ulong? CountingChannelId { get; }

        [JsonProperty("LastSuccessfulNumber", Required = Required.AllowNull)]
        public ulong? LastSuccessfulNumber { get; }

        [JsonProperty("LastUserWhoCounted", Required = Required.AllowNull)]
        public ulong? LastUserWhoCounted { get; }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        public ulong? HighestCountEver { get; }

        [JsonProperty("OverrideCommandPermissionsConstraint", Required = Required.Default)]
        public IReadOnlyDictionary<string, GuildPermission[]> OverrideCommandPermissionsConstraint { get; set; } = new Dictionary<string, GuildPermission[]>();

        [JsonProperty("OverrideCommandRolesConstraint", Required = Required.Default)]
        public IReadOnlyDictionary<string, ulong[]> OverrideCommandRolesConstraint { get; set; } = new Dictionary<string, ulong[]>();
    }
}
