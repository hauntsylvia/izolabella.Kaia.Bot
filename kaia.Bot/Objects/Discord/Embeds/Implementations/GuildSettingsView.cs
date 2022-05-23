using Discord;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations
{
    internal class GuildSettingsView : CCBEmbed
    {
        public GuildSettingsView(string GuildName, CCB_Structures.CCBGuild Guild) : base()
        {
            this.Description = $"// ***{GuildName.ToLower()}***";
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *counting channel*\n <#{Guild.Settings.CountingChannelId}>",
            });
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *highest number counted*\n `{Guild.Settings.HighestCountEver ?? 0}`",
            });
        }
    }
}
