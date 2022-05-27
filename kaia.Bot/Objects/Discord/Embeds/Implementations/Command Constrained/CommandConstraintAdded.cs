﻿using Discord;
using Discord.WebSocket;
using System.Text.RegularExpressions;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    internal class CommandConstraintAdded : KaiaPathEmbed
    {
        public CommandConstraintAdded(SocketGuild Guild, string CommandName, ulong[]? Roles = null, GuildPermission[]? Permissions = null) : base(Guild.Name, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            if (Permissions != null)
            {
                List<string> PermsStrs = new();
                foreach (GuildPermission P in Permissions)
                {
                    PermsStrs.Add(Regex.Replace(P.ToString(), "([A-Z])", " $1").ToLower(CultureInfo.InvariantCulture));
                }
                this.WriteListToOneField("required permissions", PermsStrs, ", ");
            }
            if (Roles != null)
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
                this.WriteField("required roles", $"{(RoleStr != Strings.EmbedStrings.Empty ? $"{RoleStr} {(MissingRolesCount > 0 ? "and " : "")} " : "")} {(MissingRolesCount > 0 ? $"{MissingRolesCount} unidentifiable role{(MissingRolesCount != 1 ? "s" : "")}." : "")}");
            }
        }
    }
}
