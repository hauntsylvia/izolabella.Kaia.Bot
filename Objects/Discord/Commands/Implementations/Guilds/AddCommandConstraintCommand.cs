using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Guilds;

public class AddCommandConstraintCommand : KaiaCommand
{
    public override string Name => "Add Command Constraint";

    public override string Description => "[Admin] Constrain a command in your guild to certain roles or permissions.";

    public override bool GuildsOnly => true;

    public override List<IzolabellaCommandParameter> Parameters { get; } = new()
    {
        new("Allowed Role", "The role to add or overwrite to the list of allowed roles.", ApplicationCommandOptionType.Role, false),
        new("Permissions Allowed", "The role to copy the permissions of.", ApplicationCommandOptionType.Role, false),
        new("Overwrite", "If true, the current constraints will get entirely replaced by the new ones.", ApplicationCommandOptionType.Boolean, true),
    };

    public override List<IIzolabellaCommandConstraint> Constraints { get; } = new()
    {
        new WhitelistPermissionsConstraint(true, GuildPermission.Administrator)
    };

    public override List<GuildPermission> RequiredPermissions { get; } = new();

    public override string ForeverId => CommandForeverIds.AddCommandConstraint;

    public IzolabellaCommand[]? AllCommands { get; private set; }

    public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
    {
        if (Context.UserContext.User is SocketGuildUser SUser)
        {
            KaiaGuild Guild = new(SUser.Guild.Id);
            IzolabellaCommandArgument? RoleAllowed = Arguments.FirstOrDefault(A => A.Name == "allowed-role");
            IzolabellaCommandArgument? RoleToCopyPermissionsFrom = Arguments.FirstOrDefault(A => A.Name == "permissions-allowed");
            IzolabellaCommandArgument? OverwriteArg = Arguments.FirstOrDefault(A => A.Name == "overwrite");
            IzolabellaCommandArgument? CommandArg = Arguments.FirstOrDefault(A => A.IsRequired && A.Name == "command");
            bool Overwrite = OverwriteArg != null && (OverwriteArg.Value as bool? ?? false);
            if (CommandArg != null && CommandArg.Value is string CommandId)
            {
                Dictionary<string, GuildPermission[]> PermissionsDict = new(Overwrite ? new Dictionary<string, GuildPermission[]>() : Guild.Settings.OverrideCommandPermissionsConstraint);
                Dictionary<string, ulong[]> RolesDict = new(Overwrite ? new Dictionary<string, ulong[]>() : Guild.Settings.OverrideCommandRolesConstraint);
                if (RoleToCopyPermissionsFrom != null && RoleToCopyPermissionsFrom.Value is IRole CopyFrom)
                {
                    Array AllGuildPermissions = Enum.GetValues(typeof(GuildPermission));
                    List<GuildPermission> Watch = new();
                    foreach (GuildPermission Permission in AllGuildPermissions)
                    {
                        if (CopyFrom.Permissions.Has(Permission))
                        {
                            Watch.Add(Permission);
                        }
                    }
                    if (PermissionsDict.ContainsKey(CommandId))
                    {
                        Watch.AddRange(Overwrite ? new List<GuildPermission>() : PermissionsDict[CommandId]);
                        PermissionsDict[CommandId] = Watch.ToArray();
                    }
                    else
                    {
                        PermissionsDict.Add(CommandId, Watch.ToArray());
                    }
                }
                if (RoleAllowed != null && RoleAllowed.Value is IRole RolePassed)
                {
                    List<ulong> Watch = new()
                    {
                        RolePassed.Id
                    };
                    if (RolesDict.ContainsKey(CommandId))
                    {
                        Watch.AddRange(Overwrite ? new List<ulong>() : RolesDict[CommandId]);
                        RolesDict[CommandId] = Watch.ToArray();
                    }
                    else
                    {
                        RolesDict.Add(CommandId, Watch.ToArray());
                    }
                }
                Guild.Settings.OverrideCommandPermissionsConstraint = PermissionsDict;
                Guild.Settings.OverrideCommandRolesConstraint = RolesDict;
                Guild.Settings = Guild.Settings;
                await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new CommandConstraintAdded(SUser.Guild, this.AllCommands?.FirstOrDefault(C => C is KaiaCommand CCB && CCB.ForeverId == CommandId)?.Name ?? CommandId, Guild.Settings.OverrideCommandRolesConstraint.GetValueOrDefault(CommandId), Guild.Settings.OverrideCommandPermissionsConstraint.GetValueOrDefault(CommandId)).Build());
            }
        }
    }

    public override Task OnLoadAsync(IzolabellaCommand[] AllCommands)
    {
        List<IzolabellaCommandParameterChoices> Choices = new();
        foreach (IzolabellaCommand Command in AllCommands)
        {
            if (Command is KaiaCommand CCBLevelCommand)
            {
                if (CCBLevelCommand.ForeverId != this.ForeverId)
                {
                    Choices.Add(new(CCBLevelCommand.Name, CCBLevelCommand.ForeverId));
                }
            }
        }
        this.Parameters.Add(new("Command", "The command to constrain.", ApplicationCommandOptionType.String, true)
        {
            Choices = Choices
        });
        this.AllCommands = AllCommands;
        return Task.CompletedTask;
    }
}
