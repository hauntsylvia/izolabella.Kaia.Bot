using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class ReactionRolesPage : KaiaPathEmbedRefreshable
    {
        public ReactionRolesPage(SocketGuild Guild, IEnumerable<KaiaReactionRole> Roles) : base(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
        {
            this.Guild = Guild;
            this.Roles = Roles;
        }

        public SocketGuild Guild { get; }

        public IEnumerable<KaiaReactionRole> Roles { get; }

        protected override async Task ClientRefreshAsync()
        {
            foreach (KaiaReactionRole Role in this.Roles)
            {
                IRole? RelatingRole = this.Guild.GetRole(Role.RoleId);
                if(RelatingRole != null)
                {
                    IMessage? Message = await Role.GetMessageAsync(this.Guild);
                    List<string> Display = new()
                    {
                        $"setup by <@{Role.ListerId}>",
                        $"triggered by reaction with {Role.Emote}",
                    };
                    if(Message != null)
                    {
                        Display.Add(Message.GetJumpUrl());
                    }
                    this.WithListWrittenToField($"{RelatingRole.Mention}", Display, "\n");
                }
            }
        }
    }
}
