using izolabella.Discord;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds
{
    internal class KaiaStatisticsEmbed : KaiaPathEmbed
    {
        internal KaiaStatisticsEmbed() : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Kaia)
        {
            this.WriteField("handler", $"`{ProjectInformation.ProjectCreditDisplay}`");
            this.WriteField("kaia version", $"`{KaiaSessionStatistics.Version?.Major}.{KaiaSessionStatistics.Version?.Minor}.{KaiaSessionStatistics.Version?.Build}`");
            this.WriteField("author", $"`{ProjectInformation.AuthorCreditDisplay}`");
            this.WriteField("message receiver error count", $"`{KaiaSessionStatistics.MessageReceiverFailureCount}`");
        }
    }
}
