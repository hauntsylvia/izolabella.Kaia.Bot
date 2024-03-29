﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles
{
    public class ReactionRolesPage(SocketGuild Guild, IEnumerable<KaiaReactionRole> Roles) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
    {
        public SocketGuild Guild { get; } = Guild;

        public IEnumerable<KaiaReactionRole> Roles { get; } = Roles;

        protected override async Task ClientRefreshAsync()
        {
            foreach (KaiaReactionRole Role in this.Roles)
            {
                IRole? RelatingRole = this.Guild.GetRole(Role.RoleId);
                if (RelatingRole != null)
                {
                    IMessage? Message = await Role.GetMessageAsync(this.Guild);
                    List<string> Display = new()
                {
                    $"setup by <@{Role.ListerId}>",
                    $"triggered by reaction with {Role.Emote}",
                };
                    if (Message != null)
                    {
                        Display.Add($"[message url]({Message.GetJumpUrl()})");
                    }
                    this.WithListWrittenToField($"{RelatingRole.Mention}", Display, "\n");
                }
            }
        }
    }
}