namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class ListOfItemsNotFound : KaiaPathEmbedRefreshable
    {
        public ListOfItemsNotFound() : base(Strings.EmbedStrings.FakePaths.Global)
        {

        }

        public override Task ClientRefreshAsync()
        {
            this.WithField("?", "No items found!");
            this.WithField("204", "Yup, u just got an HTTP code from a Discord bot.");
            return Task.CompletedTask;
        }
    }
}
