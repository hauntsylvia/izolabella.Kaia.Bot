using Discord.WebSocket;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    public class CommandConstrainedByRoleIds : KaiaPathEmbed
    {
        public CommandConstrainedByRoleIds(string CommandName, SocketGuild Guild, params ulong[] RoleIdsRequired) : base(Guild.Name, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            int MissingRolesCount = 0;
            string RoleStr = Strings.EmbedStrings.Empty;
            foreach (ulong RoleId in RoleIdsRequired)
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
                Value = $"// *missing roles*\n {(RoleStr != Strings.EmbedStrings.Empty ? $"{RoleStr} {(MissingRolesCount > 0 ? "and " : "")} " : "")} {(MissingRolesCount > 0 ? $"{MissingRolesCount} unidentifiable role{(MissingRolesCount != 1 ? "s" : "")}." : "")}",
            });
        }
    }
}
