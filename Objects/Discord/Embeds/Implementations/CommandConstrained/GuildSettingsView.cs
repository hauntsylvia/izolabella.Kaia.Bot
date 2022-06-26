namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    internal class GuildSettingsView : KaiaPathEmbed
    {
        public GuildSettingsView(string GuildName, KaiaGuild Guild) : base(GuildName, Strings.EmbedStrings.FakePaths.Settings)
        {
            this.WithField("counting channel", $"<#{Guild.Settings.CountingChannelId}>");
            this.WithField("highest number counted", $"`{Guild.Settings.HighestCountEver ?? 0}`");
        }
    }
}
