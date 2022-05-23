using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Base
{
    public class CCBEmbed : EmbedBuilder
    {
        public CCBEmbed(Color? Override = null) : base()
        {
            this.Color = Override ?? Colors.EmbedColor;
            this.Footer = new()
            {
                Text = Strings.EmbedStrings.FooterString,
            };
            this.Timestamp = Strings.EmbedStrings.DefaultTimestamp;
        }
    }
}
