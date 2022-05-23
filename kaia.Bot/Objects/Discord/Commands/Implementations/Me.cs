using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants;
using Kaia.Bot.Objects.Discord.Commands.Bases;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class Me : ICCBCommand
    {
        public string Name => "Me";

        public string Description => "View my statistics.";

        public List<IzolabellaCommandParameter> Parameters => new();
        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.MeCommand;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            await Context.UserContext.RespondAsync(text: "", embed: new MeView(Context.UserContext.User.Username, await CCBUser.GetOrCreateAsync(Context.UserContext.User.Id)).Build());
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
