﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    public class CommandConstrainedByRoleIds : KaiaPathEmbedRefreshable
    {
        public CommandConstrainedByRoleIds(string CommandName, SocketGuild Guild, params ulong[] RoleIdsRequired) : base(Guild.Name, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            this.CommandName = CommandName;
            this.Guild = Guild;
            this.RoleIdsRequired = RoleIdsRequired;
        }

        public string CommandName { get; }
        public SocketGuild Guild { get; }
        public ulong[] RoleIdsRequired { get; }

        protected override Task ClientRefreshAsync()
        {
            int MissingRolesCount = 0;
            string RoleStr = Strings.EmbedStrings.Empty;
            foreach (ulong RoleId in this.RoleIdsRequired)
            {
                SocketRole? RoleOfId = this.Guild.GetRole(RoleId);
                if (RoleOfId != null)
                {
                    RoleStr += $"{(RoleStr != Strings.EmbedStrings.Empty ? ", " : "")}{RoleOfId.Mention}";
                }
                else
                {
                    MissingRolesCount++;
                }
            }
            this.WithField(Strings.EmbedStrings.Empty, $"// *missing roles*\n {(RoleStr != Strings.EmbedStrings.Empty ? $"{RoleStr} {(MissingRolesCount > 0 ? "and " : "")} " : "")} {(MissingRolesCount > 0 ? $"{MissingRolesCount} unidentifiable role{(MissingRolesCount != 1 ? "s" : "")}." : "")}");
            return Task.CompletedTask;
        }
    }
}