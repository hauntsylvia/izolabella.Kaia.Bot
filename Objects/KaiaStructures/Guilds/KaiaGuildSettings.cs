using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaGuildSettings
    {
        [JsonConstructor]
        public KaiaGuildSettings(ulong? CountingChannelId = null, ulong? LastSuccessfulNumber = null, ulong? LastUserWhoCounted = null, ulong? HighestCountEver = null, IReadOnlyDictionary<string, GuildPermission[]>? OverrideCommandPermissionsConstraint = null, List<KaiaReactionRole>? ReactionRoles = null)
        {
            this.CountingChannelId = CountingChannelId;
            this.lastSuccessfulNumber = LastSuccessfulNumber;
            this.lastUserWhoCounted = LastUserWhoCounted ?? this.LastUserWhoCounted;
            this.highestCountEver = HighestCountEver ?? this.HighestCountEver;
            this.OverrideCommandPermissionsConstraint = OverrideCommandPermissionsConstraint ?? this.OverrideCommandPermissionsConstraint;
            this.reactionRoles = ReactionRoles ?? new();
        }

        [JsonProperty(nameof(CountingChannelId), Required = Required.AllowNull)]
        public ulong? CountingChannelId { get; set; }

        [JsonProperty(nameof(LastSuccessfulNumber), Required = Required.AllowNull)]
        private ulong? lastSuccessfulNumber;
        public ulong LastSuccessfulNumber { get => this.lastSuccessfulNumber ?? 0; set => this.lastSuccessfulNumber = value; }

        [JsonProperty(nameof(LastUserWhoCounted), Required = Required.AllowNull)]
        private ulong? lastUserWhoCounted;
        public ulong? LastUserWhoCounted { get => this.lastUserWhoCounted ?? 0; set => this.lastUserWhoCounted = value; }

        [JsonProperty(nameof(HighestCountEver), Required = Required.Default)]
        private ulong? highestCountEver;
        public ulong? HighestCountEver { get => this.highestCountEver ?? 0; set => this.highestCountEver = value; }

        [JsonProperty(nameof(OverrideCommandPermissionsConstraint), Required = Required.Default)]
        public IReadOnlyDictionary<string, GuildPermission[]> OverrideCommandPermissionsConstraint { get; set; } = new Dictionary<string, GuildPermission[]>();

        [JsonProperty(nameof(OverrideCommandRolesConstraint), Required = Required.Default)]
        public IReadOnlyDictionary<string, ulong[]> OverrideCommandRolesConstraint { get; set; } = new Dictionary<string, ulong[]>();

        [JsonProperty(nameof(ReactionRoles), Required = Required.Default)]
        private List<KaiaReactionRole> reactionRoles = new();
        public List<KaiaReactionRole> ReactionRoles
        {
            get
            {
                this.reactionRoles = this.reactionRoles.DistinctBy(A => A.RoleId).ToList();
                return this.reactionRoles;
            }
        }

        [JsonProperty("AutoRole", Required = Required.Default)]
        private List<KaiaAutoRole> autoRoles = new();
        public List<KaiaAutoRole> AutoRoles
        {
            get
            {
                this.autoRoles = this.autoRoles.DistinctBy(A => A.RoleId).ToList();
                return this.autoRoles;
            }
        }
    }
}