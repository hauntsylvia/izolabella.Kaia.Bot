using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.Pending;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Self;

public class RelationshipsPending : KaiaSubCommand
{
    public override string Name => "Pending";

    public override string Description => "View pending relationship invites.";

    public override bool GuildsOnly => false;

    public override List<IzolabellaCommandParameter> Parameters => new();

    public override List<IIzolabellaCommandConstraint> Constraints => new();

    public override string ForeverId => CommandForeverIds.RelationshipsPending;

    public override List<GuildPermission> RequiredPermissions => new();

    public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
    {
        await new PendingRelationshipInvitesPaginated(Context).StartAsync();
    }
}
