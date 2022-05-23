using Discord;
using Discord.WebSocket;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Base;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations
{
    internal class CommandConstraintAdded : CCBEmbed
    {
        public CommandConstraintAdded(SocketGuild Guild, string CommandName, ulong[]? Roles = null, GuildPermission[]? Permissions = null) : base()
        {
            this.Description = $"*{Guild.Name}* // ***{CommandName.ToLower()}***";
            if(Permissions != null)
            {
                string PermsStr = Strings.EmbedStrings.Empty;
                foreach (GuildPermission P in Permissions)
                {
                    PermsStr += $"{(PermsStr != Strings.EmbedStrings.Empty ? ", " : "")}{Regex.Replace(P.ToString(), "([A-Z])", " $1").ToLower()}";
                }
                this.Fields.Add(new()
                {
                    Name = $"{Strings.EmbedStrings.Empty}",
                    Value = $"// *required permissions*\n {PermsStr}",
                });
            }
            if(Roles != null)
            {
                int MissingRolesCount = 0;
                string RoleStr = Strings.EmbedStrings.Empty;
                foreach (ulong RoleId in Roles)
                {
                    SocketRole? RoleOfId = Guild.GetRole(RoleId);
                    if (RoleOfId != null)
                    {
                        RoleStr += $"{(RoleStr != Strings.EmbedStrings.Empty ? ", " : "")}{RoleOfId.Mention}";
                    }
                    else
                    {
                        MissingRolesCount++;
                    }
                }
                this.Fields.Add(new()
                {
                    Name = $"{Strings.EmbedStrings.Empty}",
                    Value = $"// *required roles*\n {(RoleStr != Strings.EmbedStrings.Empty ? $"{RoleStr} {(MissingRolesCount > 0 ? "and " : "")} " : "")} {(MissingRolesCount > 0 ? $"{MissingRolesCount} unidentifiable role{(MissingRolesCount != 1 ? "s" : "")}." : "")}",
                });
            }
        }
    }
}
