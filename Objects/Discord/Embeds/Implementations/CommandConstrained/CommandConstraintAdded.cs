using System.Text.RegularExpressions;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    internal class CommandConstraintAdded : KaiaPathEmbedRefreshable
    {
        public CommandConstraintAdded(SocketGuild Guild, string CommandName, ulong[]? Roles = null, GuildPermission[]? Permissions = null) : base(Guild.Name, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            this.Guild = Guild;
            this.CommandName = CommandName;
            this.Roles = Roles;
            this.Permissions = Permissions;
        }

        public SocketGuild Guild { get; }

        public string CommandName { get; }

        public ulong[]? Roles { get; }

        public GuildPermission[]? Permissions { get; }

        public override Task ClientRefreshAsync()
        {
            if (this.Permissions != null)
            {
                List<string> PermsStrs = new();
                foreach (GuildPermission P in this.Permissions)
                {
                    PermsStrs.Add(Regex.Replace(P.ToString(), "([A-Z])", " $1").ToLower(CultureInfo.InvariantCulture));
                }
                this.WithListWrittenToField("required permissions", PermsStrs, ", ");
            }
            if (this.Roles != null)
            {
                int MissingRolesCount = 0;
                string RoleStr = Strings.EmbedStrings.Empty;
                foreach (ulong RoleId in this.Roles)
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
                this.WithField("required roles", $"{(RoleStr != Strings.EmbedStrings.Empty ? $"{RoleStr} {(MissingRolesCount > 0 ? "and " : "")} " : "")} {(MissingRolesCount > 0 ? $"{MissingRolesCount} unidentifiable role{(MissingRolesCount != 1 ? "s" : "")}." : "")}");
            }
            return Task.CompletedTask;
        }
    }
}
