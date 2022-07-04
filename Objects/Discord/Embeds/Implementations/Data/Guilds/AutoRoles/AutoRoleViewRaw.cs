using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles
{
    public class AutoRoleViewRaw : KaiaPathEmbedRefreshable
    {
        public AutoRoleViewRaw(CommandContext Context, SocketGuild Guild, KaiaAutoRole Role) : base(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
        {
            this.Guild = Guild;
            this.Role = Role;
        }

        public SocketGuild Guild { get; }

        public KaiaAutoRole Role { get; }

        protected override async Task ClientRefreshAsync()
        {
            IRole? GuildRole = await this.Role.GetRoleAsync(this.Guild);
            this.WithField("role mention", GuildRole?.Mention ?? "unknown");
            this.WithField("author id", this.Role.ListerId.ToString(CultureInfo.InvariantCulture));
            this.WithField("enforced", this.Role.Enforce.ToString());
        }
    }
}
