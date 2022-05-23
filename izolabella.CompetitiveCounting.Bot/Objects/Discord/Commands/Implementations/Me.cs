using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Bases;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Implementations
{
    public class Me : ICCBCommand
    {
        public string Name => "Me";

        public string Description => "View my statistics.";

        public List<IzolabellaCommandParameter> Parameters => new();
        public List<IIzolabellaCommandConstraint> Constraints => new();

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            await Context.UserContext.RespondAsync(text: "", embed: new MeView(Context.UserContext.User.Username, await CCBUser.GetOrCreateAsync(Context.UserContext.User.Id)).Build());
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
