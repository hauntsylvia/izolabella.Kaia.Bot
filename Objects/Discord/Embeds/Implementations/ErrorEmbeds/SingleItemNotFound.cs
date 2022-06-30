namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class SingleItemNotFound : KaiaPathEmbedRefreshable
    {
        public SingleItemNotFound() : base(Strings.EmbedStrings.FakePaths.Global)
        {

        }

        public override Task RefreshAsync()
        {
            this.WithField("404", "Nothing is here.");
            return Task.CompletedTask;
        }
    }
}
