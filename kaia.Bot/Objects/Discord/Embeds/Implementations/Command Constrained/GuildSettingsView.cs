using Kaia.Bot.Objects.KaiaStructures;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    internal class GuildSettingsView : KaiaPathEmbed
    {
        public GuildSettingsView(string GuildName, KaiaGuild Guild) : base(GuildName, Strings.EmbedStrings.FakePaths.Settings)
        {
            this.WriteField("counting channel", $"<#{Guild.Settings.CountingChannelId}>");
            this.WriteField("highest number counted", $"`{Guild.Settings.HighestCountEver ?? 0}`");
        }
    }
}
