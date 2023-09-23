using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles
{
    public class ReactionRolesViewRaw(SocketGuild Guild, KaiaReactionRole Role) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
    {
        public SocketGuild Guild { get; } = Guild;

        public KaiaReactionRole Role { get; } = Role;

        protected override async Task ClientRefreshAsync()
        {
            IRole? GuildRole = await this.Role.GetRoleAsync(this.Guild);
            IMessage? GuildMessage = await this.Role.GetMessageAsync(this.Guild);
            this.WithField("message link", GuildMessage?.GetJumpUrl() ?? "unknown");
            this.WithField("role mention", GuildRole?.Mention ?? "unknown");
            this.WithField("author id", this.Role.ListerId.ToString(CultureInfo.InvariantCulture));
            this.WithField("listening emote", this.Role.Emote.ToString());
        }
    }
}