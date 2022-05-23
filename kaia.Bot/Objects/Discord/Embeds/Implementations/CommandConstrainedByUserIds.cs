using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations
{
    public class CommandConstrainedByUserIds : CCBEmbed
    {
        public CommandConstrainedByUserIds(string CommandName) : base()
        {
            this.Description = $"// ***{CommandName.ToLower()}***";
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *access*\nYou do not have access to this command.",
            });
        }
    }
}
