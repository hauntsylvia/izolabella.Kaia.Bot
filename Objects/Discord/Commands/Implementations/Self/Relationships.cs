using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Self
{
    public class Relationships : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.Relationships;

        public override List<GuildPermission> RequiredPermissions => new();

        public override string Name => "Relationships";

        public override string Description => "View your relationships";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters => new()
        {
            new("View Pending", "Whether to view current or pending relationships.", ApplicationCommandOptionType.Boolean, false)
        };

        public override List<IIzolabellaCommandConstraint> Constraints => new();

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? PArg = Arguments.FirstOrDefault(A => A.Name == "view-pending");
            if(PArg != null && PArg.Value is bool ViewPending && ViewPending)
            {
                await new PendingRelationshipInvitesPaginated(Context).StartAsync();
            }
            else
            {
                await new MyRelationshipsPaginated(Context).StartAsync();
            }
        }
    }
}
