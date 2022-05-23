using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations
{
    public class CommandConstrainedByRoleIds : CCBEmbed
    {
        public CommandConstrainedByRoleIds(string CommandName, SocketGuild Guild, params ulong[] RoleIdsRequired) : base()
        {
            this.Description = $"// ***{CommandName.ToLower()}***";
            int MissingRolesCount = 0;
            string MissingStr = Strings.EmbedStrings.Empty;
            foreach (ulong RoleId in RoleIdsRequired)
            {
                SocketRole? RoleOfId = Guild.GetRole(RoleId);
                if(RoleOfId != null)
                {
                    MissingStr += $"{(MissingStr != Strings.EmbedStrings.Empty ? ", " : "")}{RoleOfId.Mention}";
                }
                else
                {
                    MissingRolesCount++;
                }
            }
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *missing permissions*\n {(MissingStr != Strings.EmbedStrings.Empty ? $"{MissingStr} {(MissingRolesCount > 0 ? "and " : "")} " : "")} {(MissingRolesCount > 0 ? $"{MissingRolesCount} unidentifiable role{(MissingRolesCount != 1 ? "s" : "")}." : "")}",
            });
        }
    }
}
