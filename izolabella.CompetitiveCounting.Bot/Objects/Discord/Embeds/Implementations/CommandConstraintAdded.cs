using Discord;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Base;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations
{
    internal class CommandConstraintAdded : CCBEmbed
    {
        public CommandConstraintAdded(string GuildName, string CommandName, IIzolabellaCommandConstraint AddedConstraint) : base()
        {
            this.Description = $"*{GuildName}* // ***{CommandName.ToLower()}***";
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *constraint type*\n `{AddedConstraint.Type}`",
            });
        }
    }
}
