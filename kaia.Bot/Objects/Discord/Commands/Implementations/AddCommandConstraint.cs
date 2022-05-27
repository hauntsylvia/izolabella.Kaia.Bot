using Discord;
using Discord.WebSocket;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Embeds.Implementations;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class AddCommandConstraint : ICCBCommand
    {
        public string Name => "Add Command Constraint";

        public string Description => "[Admin] Constrain a command in my guild to certain roles or permissions.";

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Allowed Role", "The role to add or overwrite to the list of allowed roles.", ApplicationCommandOptionType.Role, false),
            new("Permissions Allowed", "The role to copy the permissions of.", ApplicationCommandOptionType.Role, false),
            new("Overwrite", "If true, the current constraints will get entirely replaced by the new ones.", ApplicationCommandOptionType.Boolean, true),
        };
        public List<IIzolabellaCommandConstraint> Constraints { get; } = new()
        {
            new WhitelistPermissionsConstraint(true, GuildPermission.Administrator)
        };

        public string ForeverId => CommandForeverIds.AddCommandConstraint;

        public IIzolabellaCommand[]? AllCommands { get; private set; }

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
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
                    await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new CommandConstraintAdded(SUser.Guild, this.AllCommands?.FirstOrDefault(C =>
                    {
                        return C is ICCBCommand CCB && CCB.ForeverId == CommandId;
                    })?.Name ?? CommandId, Guild.Settings.OverrideCommandRolesConstraint.GetValueOrDefault(CommandId), Guild.Settings.OverrideCommandPermissionsConstraint.GetValueOrDefault(CommandId)).Build());
                }
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            List<IzolabellaCommandParameterChoices> Choices = new();
            foreach (IIzolabellaCommand Command in AllCommands)
            {
                if (Command is ICCBCommand CCBLevelCommand)
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

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
