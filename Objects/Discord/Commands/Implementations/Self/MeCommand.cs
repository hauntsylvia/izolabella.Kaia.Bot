using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeData;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Self
{
    public class MeCommand : KaiaCommand
    {
        public override string Name => "MeData";

        public override string Description => "View my statistics and my inventory, or view another user's statistics.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters => new()
        {
            CommandParameters.SomeoneOtherThanMeUser,
        };

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override string ForeverId => CommandForeverIds.MeCommand;

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? UserArg = Arguments.FirstOrDefault(A => A.Name.ToLower(CultureInfo.InvariantCulture) == CommandParameters.SomeoneOtherThanMeUser.Name.ToLower(CultureInfo.InvariantCulture));
            IUser U = UserArg != null && UserArg.Value is IUser DU ? DU : Context.UserContext.User;
            if (U.Id == Context.UserContext.User.Id)
            {
                await new MeInventoryView(new(U.Id), Context, 4).StartAsync();
            }
            else
            {
                MeView? M = new(U.Username, new(U.Id));
                await M.RefreshAsync();
                await Context.UserContext.RespondAsync(text: "", embed: M.Build());
            }
        }
    }
}
