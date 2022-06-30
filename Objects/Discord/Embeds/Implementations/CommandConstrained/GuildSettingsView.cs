namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    public class GuildSettingsView : KaiaPathEmbedRefreshable
    {
        public GuildSettingsView(string GuildName, KaiaGuild Guild) : base(GuildName, Strings.EmbedStrings.FakePaths.Settings)
        {
            this.GuildName = GuildName;
            this.Guild = Guild;
        }

        public string GuildName { get; }

        public KaiaGuild Guild { get; }

        public override Task ClientRefreshAsync()
        {
            this.WithField("counting channel", $"<#{this.Guild.Settings.CountingChannelId}>");
            this.WithField("highest number counted", $"`{this.Guild.Settings.HighestCountEver ?? 0}`");
            return Task.CompletedTask;
        }
    }
}
