using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations
{
    public class CommandConstrainedByPermissions : CCBEmbed
    {
        public CommandConstrainedByPermissions(string CommandName, GuildPermissions UserHas, GuildPermission[] Required) : base()
        {
            this.Description = $"// ***{CommandName.ToLower()}***";
            string MissingStr = Strings.EmbedStrings.Empty;
            GuildPermission[] MissingGP = Required.Where(P => !UserHas.Has(P)).ToArray();
            foreach (GuildPermission P in MissingGP)
            {
                MissingStr += $"{(MissingStr != Strings.EmbedStrings.Empty ? ", " : "")}{P.ToString().ToLower()}";
            }
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *missing permissions*\n {MissingStr}",
            });
        }
    }
}
