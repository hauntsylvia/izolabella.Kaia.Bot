using Discord;
using Kaia.Bot.Objects.Constants;
using Kaia.Bot.Objects.Discord.Embeds.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    internal class GuildSettingsView : CCBPathEmbed
    {
        public GuildSettingsView(string GuildName, CCB_Structures.KaiaGuild Guild) : base(GuildName, Strings.EmbedStrings.FakePaths.Settings)
        {
            this.WriteField("counting channel", $"<#{Guild.Settings.CountingChannelId}>");
            this.WriteField("highest number counted", $"`{Guild.Settings.HighestCountEver ?? 0}`");
        }
    }
}
