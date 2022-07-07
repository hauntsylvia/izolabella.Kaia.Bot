using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.ServerManagement.Roles
{
    public class ReactionRoles : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.ReactionRoles;

        public override string Name => "Reaction Roles";

        public override string Description => "View and delete reaction roles.";

        public override bool GuildsOnly => true;

        public override List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Ephemeral", "Make the response visible to only you.", ApplicationCommandOptionType.Boolean, false)
        };

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new()
        {
            new WhitelistPermissionsConstraint(false, GuildPermission.ManageRoles)
        };

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
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
