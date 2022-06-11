using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    internal class InformationCommand : IKaiaCommand
    {
        public string ForeverId => CommandForeverIds.BotDevelopmentInformation;

        public string Name => "Information";

        public string Description => "View Kaia's statistics for this session.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new();

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

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
            await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new KaiaStatisticsEmbed().Build());
        }
    }
}
