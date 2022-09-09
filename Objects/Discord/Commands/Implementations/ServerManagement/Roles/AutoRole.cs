using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.ServerManagement.Roles;

public class AutoRole : KaiaCommand
{
    public override string ForeverId => CommandForeverIds.AutoRole;

    public override string Name => "Auto Role";

    public override string Description => "Add a role to automatically give users.";

    public override bool GuildsOnly => true;

    public override List<IzolabellaCommandParameter> Parameters { get; } = new()
    {
        new("Role", "The role to give to new members.", ApplicationCommandOptionType.Role, true),
        new("Enforce", "(Recommended: true) The role is granted or removed even when Kaia is offline (on startup).", ApplicationCommandOptionType.Boolean, true),
    };

    public override List<IIzolabellaCommandConstraint> Constraints { get; } = new()
    {
        new WhitelistPermissionsConstraint(false, GuildPermission.ManageRoles)
    };

    public override List<GuildPermission> RequiredPermissions { get; } = new()
    {
        GuildPermission.ManageRoles,
    };

    public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
    {
        IzolabellaCommandArgument? RoleArg = Arguments.FirstOrDefault(A => A.Name == "role");
        IzolabellaCommandArgument? EnforceArg = Arguments.FirstOrDefault(A => A.Name == "enforce");
        if (RoleArg != null && RoleArg.Value is IRole Role && EnforceArg != null && EnforceArg.Value is bool Enforce && Context.UserContext.User is SocketGuildUser User)
        {
            KaiaAutoRole NewAutoRole = new(Context.UserContext.User.Id, Role.Id, Enforce);
            KaiaGuild G = new(User.Guild.Id);
            G.Settings.AutoRoles.Add(NewAutoRole);
            await G.SaveAsync();
            await new AutoRolesPaginated(Context, User.Guild, false).StartAsync();
        }
    }
}
