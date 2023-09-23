using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles
{
    public class AutoRolesPage(SocketGuild Guild, IEnumerable<KaiaAutoRole> Roles) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
    {
        public SocketGuild Guild { get; } = Guild;

        public IEnumerable<KaiaAutoRole> Roles { get; } = Roles;

        protected override Task ClientRefreshAsync()
        {
            foreach (KaiaAutoRole Role in this.Roles)
            {
                IRole? RelatingRole = this.Guild.GetRole(Role.RoleId);
                if (RelatingRole != null)
                {
                    List<string> Display = new()
                {
                    $"setup by <@{Role.ListerId}>",
                };
                    this.WithListWrittenToField($"{RelatingRole.Mention}", Display, "\n");
                }
            }
            return Task.CompletedTask;
        }
    }
}