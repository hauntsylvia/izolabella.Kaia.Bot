using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class SingleItemNotFound : KaiaPathEmbedRefreshable
    {
        public SingleItemNotFound() : base(Strings.EmbedStrings.FakePaths.Global)
        {
        }

        protected override Task ClientRefreshAsync()
        {
            this.WithField("404", "Nothing is here.");
            return Task.CompletedTask;
        }
    }
}