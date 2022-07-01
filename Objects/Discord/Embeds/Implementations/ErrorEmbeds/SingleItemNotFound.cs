namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class SingleItemNotFound : KaiaPathEmbedRefreshable
    {
        public SingleItemNotFound() : base(Strings.EmbedStrings.FakePaths.Global)
        {
            this.WithField("404", "Nothing is here.");
        }

        protected override Task ClientRefreshAsync()
        {
            return Task.CompletedTask;
        }
    }
}
