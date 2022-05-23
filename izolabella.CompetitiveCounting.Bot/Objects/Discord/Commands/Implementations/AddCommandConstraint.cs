using Discord;
using Discord.WebSocket;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Bases;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Implementations
{
    public class AddCommandConstraint : ICCBCommand
    {
        public string Name => "Add Command Constraint";

        public string Description => "Constrain a command in my guild to certain roles or permissions.";

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Add Allowed Role", "The role to add to the list of allowed roles.", ApplicationCommandOptionType.Role, false),
            new("Permissions Allowed", "The role to copy the permissions of.", ApplicationCommandOptionType.Role, false),
        };
        public List<IIzolabellaCommandConstraint> Constraints => new();

        public string ForeverId => CommandForeverIds.AddCommandConstraint;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            if(Context.UserContext.User is SocketGuildUser SUser)
            {
                CCBGuild Guild = await CCBGuild.GetOrCreateAsync(SUser.Guild.Id);
                IzolabellaCommandArgument? RoleAllowed = Arguments.FirstOrDefault(A => A.Name == "add-allowed-role");
                IzolabellaCommandArgument? RoleToCopyPermissionsFrom = Arguments.FirstOrDefault(A => A.Name == "permissions-allowed");
                if(Arguments.First(A => A.IsRequired).Value is string CommandId)
                {
                    if(RoleToCopyPermissionsFrom != null && RoleToCopyPermissionsFrom.Value is IRole CopyFrom)
                    {
                        Array AllGuildPermissions = Enum.GetValues(typeof(GuildPermission));
                        List<GuildPermission> Watch = new();
                        foreach(GuildPermission Permission in AllGuildPermissions)
                        {
                            if(CopyFrom.Permissions.Has(Permission))
                            {
                                Watch.Add(Permission);
                            }
                        }
                        Dictionary<string, GuildPermission[]> New = new(Guild.Settings.OverrideCommandPermissionsConstraint)
                        {
                            { CommandId, Watch.ToArray() }
                        };
                        Guild.Settings.OverrideCommandPermissionsConstraint = New;
                    }
                    Guild = await Guild.ChangeGuildSettings(Guild.Settings);
                }
            }
            else
            {

            }
            await Context.UserContext.RespondAsync(text: "", embed: new MeView(Context.UserContext.User.Username, await CCBUser.GetOrCreateAsync(Context.UserContext.User.Id)).Build());
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            List<IzolabellaCommandParameterChoices> Choices = new();
            foreach(IIzolabellaCommand Command in AllCommands)
            {
                if(Command is ICCBCommand CCBLevelCommand)
                {
                    if(CCBLevelCommand.ForeverId != this.ForeverId)
                    {
                        Choices.Add(new(CCBLevelCommand.Name, CCBLevelCommand.ForeverId));
                    }
                }
            }
            this.Parameters.Add(new("Command", "The command to constrain.", ApplicationCommandOptionType.String, true)
            {
                Choices = Choices
            });
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
