using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles
{
    public class ReactionRolesViewRaw : KaiaPathEmbedRefreshable
    {
        public ReactionRolesViewRaw(SocketGuild Guild, KaiaReactionRole Role) : base(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
        {
            this.Guild = Guild;
            this.Role = Role;
        }

        public SocketGuild Guild { get; }

        public KaiaReactionRole Role { get; }

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
