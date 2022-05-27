﻿using Discord;
using System.Text.RegularExpressions;

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
