using izolabella.Discord;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds
{
    public class KaiaStatisticsEmbed : KaiaPathEmbedRefreshable
    {
        public KaiaStatisticsEmbed() : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Kaia)
        {
            ClientRefreshAsync().Wait();
        }

        protected override Task ClientRefreshAsync()
        {
            WithField("handler", $"`{ProjectInformation.ProjectCreditDisplay}`");
            WithField("kaia version", $"`{KaiaSessionStatistics.Version?.Major}.{KaiaSessionStatistics.Version?.Minor}.{KaiaSessionStatistics.Version?.Build}`");
            WithField("author", $"`{ProjectInformation.AuthorCreditDisplay}`");
            WithField("message receiver error count", $"`{KaiaSessionStatistics.MessageReceiverFailureCount}`");
            return Task.CompletedTask;
        }
    }
}
