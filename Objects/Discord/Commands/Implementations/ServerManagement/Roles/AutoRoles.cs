﻿using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.ServerManagement.Roles
{
    public class AutoRoles : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.AutoRoles;

        public override string Name => "Auto Roles";

        public override string Description => "View and delete auto roles.";

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
                await new AutoRolesPaginated(Context, User.Guild, Ephemeral).StartAsync();
            }
        }
    }
}