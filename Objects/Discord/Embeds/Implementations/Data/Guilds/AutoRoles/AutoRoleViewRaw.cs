using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles
{
    public class AutoRoleViewRaw : KaiaPathEmbedRefreshable
    {
        public AutoRoleViewRaw(SocketGuild Guild, KaiaAutoRole Role) : base(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
        {
            this.Guild = Guild;
            this.Role = Role;
        }

        public SocketGuild Guild { get; }

        public KaiaAutoRole Role { get; }

        protected override async Task ClientRefreshAsync()
        {
            IRole? GuildRole = await Role.GetRoleAsync(Guild);
            WithField("role mention", GuildRole?.Mention ?? "unknown");
            WithField("author id", Role.ListerId.ToString(CultureInfo.InvariantCulture));
            WithField("enforced", Role.Enforce.ToString());
        }
    }
}
