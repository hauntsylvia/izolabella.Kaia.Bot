using izolabella.Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds
{
    internal class KaiaStatisticsEmbed : KaiaPathEmbed
    {
        internal KaiaStatisticsEmbed() : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Kaia)
        {
            this.WriteField("handler", ProjectInformation.ProjectCreditDisplay);
            this.WriteField("author", ProjectInformation.AuthorCreditDisplay);
            this.WriteField("message receiver error count", $"{KaiaSessionStatistics.MessageReceiverFailureCount}");
            this.WriteField("session startup", $"{KaiaSessionStatistics.SessionStartupAt.ToLongDateString()} - {KaiaSessionStatistics.SessionStartupAt.ToShortTimeString()} [UTC]");
        }
    }
}
