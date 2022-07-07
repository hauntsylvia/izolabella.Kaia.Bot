namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class ListOfItemsNotFound : KaiaPathEmbedRefreshable
    {
        public ListOfItemsNotFound() : base(Strings.EmbedStrings.FakePaths.Global)
        {
        }

        protected override Task ClientRefreshAsync()
        {
            this.WithField("?", "No items found!");
            this.WithField("What can you do?", "normally, this just means there is nothing to view! " +
                "if there *should be* items to view (like the library), then this is an error. " +
                "if not, then this is intentional. __if you need support, please [join this server](https://discord.gg/fe8UXqyTSb).__", true);
            return Task.CompletedTask;
        }
    }
}
