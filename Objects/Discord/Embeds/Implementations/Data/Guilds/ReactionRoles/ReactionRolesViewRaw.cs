﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles
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
            IRole? GuildRole = await Role.GetRoleAsync(Guild);
            IMessage? GuildMessage = await Role.GetMessageAsync(Guild);
            WithField("message link", GuildMessage?.GetJumpUrl() ?? "unknown");
            WithField("role mention", GuildRole?.Mention ?? "unknown");
            WithField("author id", Role.ListerId.ToString(CultureInfo.InvariantCulture));
            WithField("listening emote", Role.Emote.ToString());
        }
    }
}
