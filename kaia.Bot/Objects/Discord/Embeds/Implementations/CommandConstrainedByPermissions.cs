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
    public class CommandConstrainedByPermissions : CCBPathEmbed
    {
        public CommandConstrainedByPermissions(string GuildName, string CommandName, GuildPermissions UserHas, GuildPermission[] Required) : base(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            string MissingStr = Strings.EmbedStrings.Empty;
            GuildPermission[] MissingGP = Required.Where(P => !UserHas.Has(P)).ToArray();
            foreach (GuildPermission P in MissingGP)
            {
                MissingStr += $"{(MissingStr != Strings.EmbedStrings.Empty ? ", " : "")}{Regex.Replace(P.ToString(), "([A-Z])", " $1").ToLower()}";
            }
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *missing permissions*\n {MissingStr}",
            });
        }
    }
}
