using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles
{
    public class AutoRoleViewRaw(SocketGuild Guild, KaiaAutoRole Role) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
    {
        public SocketGuild Guild { get; } = Guild;

        public KaiaAutoRole Role { get; } = Role;

        protected override async Task ClientRefreshAsync()
        {
            IRole? GuildRole = await this.Role.GetRoleAsync(this.Guild);
            this.WithField("role mention", GuildRole?.Mention ?? "unknown");
            this.WithField("author id", this.Role.ListerId.ToString(CultureInfo.InvariantCulture));
            this.WithField("enforced", this.Role.Enforce.ToString());
        }
    }
}