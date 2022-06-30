namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class RateLimited : KaiaPathEmbedRefreshable
    {
        public RateLimited() : base(Strings.EmbedStrings.FakePaths.Global)
        {
            this.WithField("Slow down", "Wait.");
        }

        public override Task ClientRefreshAsync()
        {
            return Task.CompletedTask;
        }
    }
}
