using Discord;

namespace Kaia.Bot.Objects.KaiaStructures.Guilds
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaGuildSettings
    {
        [JsonConstructor]
        public KaiaGuildSettings(ulong? CountingChannelId = null, ulong? LastSuccessfulNumber = null, ulong? LastUserWhoCounted = null, ulong? HighestCountEver = null, IReadOnlyDictionary<string, GuildPermission[]>? OverrideCommandPermissionsConstraint = null)
        {
            this.CountingChannelId = CountingChannelId;
            this.lastSuccessfulNumber = LastSuccessfulNumber;
            this.lastUserWhoCounted = LastUserWhoCounted ?? this.LastUserWhoCounted;
            this.highestCountEver = HighestCountEver ?? this.HighestCountEver;
            this.OverrideCommandPermissionsConstraint = OverrideCommandPermissionsConstraint ?? this.OverrideCommandPermissionsConstraint;
        }

        [JsonProperty("CountingChannelId", Required = Required.AllowNull)]
        public ulong? CountingChannelId { get; set; }

        [JsonProperty("LastSuccessfulNumber", Required = Required.AllowNull)]
        private ulong? lastSuccessfulNumber;
        public ulong LastSuccessfulNumber { get => this.lastSuccessfulNumber ?? 0; set => this.lastSuccessfulNumber = value; }

        [JsonProperty("LastUserWhoCounted", Required = Required.AllowNull)]
        private ulong? lastUserWhoCounted;
        public ulong? LastUserWhoCounted { get => this.lastUserWhoCounted ?? 0; set => this.lastUserWhoCounted = value; }

        [JsonProperty("HighestCountEver", Required = Required.Default)]
        private ulong? highestCountEver;
        public ulong? HighestCountEver { get => this.highestCountEver ?? 0; set => this.highestCountEver = value; }

        [JsonProperty("OverrideCommandPermissionsConstraint", Required = Required.Default)]
        public IReadOnlyDictionary<string, GuildPermission[]> OverrideCommandPermissionsConstraint { get; set; } = new Dictionary<string, GuildPermission[]>();

        [JsonProperty("OverrideCommandRolesConstraint", Required = Required.Default)]
        public IReadOnlyDictionary<string, ulong[]> OverrideCommandRolesConstraint { get; set; } = new Dictionary<string, ulong[]>();
    }
}
