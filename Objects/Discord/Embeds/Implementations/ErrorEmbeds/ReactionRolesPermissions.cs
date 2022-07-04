namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class ReactionRolesPermissions : KaiaPathEmbedRefreshable
    {
        public ReactionRolesPermissions(GuildPermission[] Permissions) : base(Strings.EmbedStrings.FakePaths.Global)
        {
            this.WithField("?", "Permissions!");
            this.WithField("What can I do?", "Normally, this just means there is nothing to view! " +
                "If there *should be* items to view (like the library), then this is an error. " +
                "If not, then this is intentional. __If you need support, please [join this server](https://discord.gg/fe8UXqyTSb).__");
        }

        protected override Task ClientRefreshAsync()
        {
            return Task.CompletedTask;
        }
    }
}
