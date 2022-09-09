using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.Channels;

internal class MarkNSFW : KaiaPathEmbed
{
    public MarkNSFW(string GuildName, string ChannelName, bool IsNSFW) : base(Strings.EmbedStrings.FakePaths.Guilds, GuildName, ChannelName)
    {
        this.WithField("nsfw", IsNSFW ? "`true`" : "`false`");
    }
}
