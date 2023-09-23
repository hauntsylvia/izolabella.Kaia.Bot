using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    public class CommandConstrainedByRoleIds(string CommandName, SocketGuild Guild, params ulong[] RoleIdsRequired) : KaiaPathEmbedRefreshable(Guild.Name, Strings.EmbedStrings.FakePaths.Commands, CommandName)
    {
        public string CommandName { get; } = CommandName;
        public SocketGuild Guild { get; } = Guild;
        public ulong[] RoleIdsRequired { get; } = RoleIdsRequired;

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