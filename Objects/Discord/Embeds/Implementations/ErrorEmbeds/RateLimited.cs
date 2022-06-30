namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class RateLimited : KaiaPathEmbedRefreshable
    {
        public RateLimited() : base(Strings.EmbedStrings.FakePaths.Global)
        {

        }

        public override Task RefreshAsync()
        {
            this.WithField("Slow down", "Wait.");
            return Task.CompletedTask;
        }
    }
}
