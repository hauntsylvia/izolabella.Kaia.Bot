using izolabella.Discord;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds;

public class KaiaStatisticsEmbed : KaiaPathEmbedRefreshable
{
    public KaiaStatisticsEmbed() : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Kaia)
    {
        this.ClientRefreshAsync().Wait();
    }

    protected override Task ClientRefreshAsync()
    {
        this.WithField("handler", $"`{ProjectInformation.ProjectCreditDisplay}`");
        this.WithField("kaia version", $"`{KaiaSessionStatistics.Version?.Major}.{KaiaSessionStatistics.Version?.Minor}.{KaiaSessionStatistics.Version?.Build}`");
        this.WithField("author", $"`{ProjectInformation.AuthorCreditDisplay}`");
        this.WithField("message receiver error count", $"`{KaiaSessionStatistics.MessageReceiverFailureCount}`");
        return Task.CompletedTask;
    }
}
