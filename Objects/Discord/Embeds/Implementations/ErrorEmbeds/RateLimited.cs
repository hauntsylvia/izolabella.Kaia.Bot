using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class RateLimited : KaiaPathEmbedRefreshable
    {
        public RateLimited() : base(Strings.EmbedStrings.FakePaths.Global)
        {
        }

        protected override Task ClientRefreshAsync()
        {
            WithField("Slow down", "Wait.");
            return Task.CompletedTask;
        }
    }
}
