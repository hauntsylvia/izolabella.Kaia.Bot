using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles;

public class AutoRolesPage : KaiaPathEmbedRefreshable
{
    public AutoRolesPage(SocketGuild Guild, IEnumerable<KaiaAutoRole> Roles) : base(Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles)
    {
        this.Guild = Guild;
        this.Roles = Roles;
    }

    public SocketGuild Guild { get; }

    public IEnumerable<KaiaAutoRole> Roles { get; }

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
