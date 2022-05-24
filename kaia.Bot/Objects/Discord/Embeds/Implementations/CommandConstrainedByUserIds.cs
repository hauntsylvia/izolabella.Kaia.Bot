using Discord;
using Kaia.Bot.Objects.Constants;
using Kaia.Bot.Objects.Discord.Embeds.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    public class CommandConstrainedByUserIds : CCBPathEmbed
    {
        public CommandConstrainedByUserIds(string GuildName, string CommandName) : base(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *access*\nYou do not have access to this command.",
            });
        }
    }
}
