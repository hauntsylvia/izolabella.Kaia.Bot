using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class MeCommand : IKaiaCommand
    {
        public string Name => "Me";

        public string Description => "View my statistics and my inventory, or view another user's statistics.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters => new()
        {
            CommandParameters.SomeoneOtherThanMeUser,
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.MeCommand;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? UserArg = Arguments.FirstOrDefault(A =>
            {
                return A.Name.ToLower(CultureInfo.InvariantCulture) == CommandParameters.SomeoneOtherThanMeUser.Name.ToLower(CultureInfo.InvariantCulture);
            });
            IUser U = UserArg != null && UserArg.Value is IUser DU ? DU : Context.UserContext.User;
            if (U.Id == Context.UserContext.User.Id)
            {
                await new MeInventoryView(new(U.Id), Context, 4).StartAsync();
            }
            else
            {
                await Context.UserContext.RespondAsync(text: "", embed: new MeView(U.Username, new(U.Id)).Build());
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
