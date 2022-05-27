﻿using Discord;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public class KaiaPathEmbed : EmbedBuilder
    {
        public KaiaPathEmbed(string Parent, string? Sub1 = null, string? Sub2 = null, Color? Override = null) : base()
        {
            this.Description =
                $"" +
                $"*{Parent.ToLower()}* // " +
                $"{(Sub1 != null ? $"{(Sub2 == null ? $"***{Sub1.ToLower()}***" : $"*{Sub1.ToLower()}*")}" : "")}" +
                $"{(Sub2 != null ? $" // ***{Sub2.ToLower()}***" : "")}";
            this.Color = Override ?? Colors.EmbedColor;
            this.Footer = new()
            {
                Text = Strings.EmbedStrings.FooterString,
            };
            this.Timestamp = Strings.EmbedStrings.DefaultTimestamp;
        }

        internal void WriteField(string Name, string Value)
        {
            this.AddField(Strings.EmbedStrings.Empty, $"// *{Name.ToLower()}*\n{Value.ToLower()}");
        }

        internal void WriteListToOneField(string Name, List<string> Values, string DelimBy = ", ")
        {
            string S = Strings.EmbedStrings.Empty;
            for (int Index = 0; Index < Values.Count; Index++)
            {
                S += $"{(Index != 0 ? DelimBy : string.Empty)}{Values[Index]}";
            }
            this.WriteField(Name, S);
        }
    }
}