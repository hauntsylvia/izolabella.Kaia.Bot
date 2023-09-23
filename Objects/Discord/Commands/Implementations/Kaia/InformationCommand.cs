using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Kaia
{
    internal sealed class  InformationCommand : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.BotDevelopmentInformation;

        public override string Name => "Information";

        public override string Description => "View Kaia's statistics for this session.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters { get; } = new();

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, embed: new KaiaStatisticsEmbed().Build());
        }
    }
}