using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Self
{
    public class ReactionRoles : IKaiaCommand
    {
        public string ForeverId => CommandForeverIds.ReactionRoles;

        public string Name => "Reaction Roles";

        public string Description => "View and delete reaction roles.";

        public bool GuildsOnly => true;

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Ephemeral", "Make the response visible to only you.", ApplicationCommandOptionType.Boolean, false)
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new()
        {
            new WhitelistPermissionsConstraint(false, GuildPermission.ManageRoles)
        };

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? EphemeralArg = Arguments.FirstOrDefault(A => A.Name == "ephemeral");
            bool Ephemeral = EphemeralArg != null && EphemeralArg.Value is bool E && E;
            
            if (Context.UserContext.User is SocketGuildUser User)
            {
                KaiaGuild G = new(User.Guild.Id);
                await new ReactionRolesPaginated(Context, User.Guild, Ephemeral).StartAsync();
            }
        }
    }
}
