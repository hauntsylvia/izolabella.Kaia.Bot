namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public class KaiaPathEmbed : EmbedBuilder
    {
        public KaiaPathEmbed(string Parent, string? Sub1 = null, string? Sub2 = null, Color? Override = null) : base()
        {
            this.Description =
                $"" +
                $"*{Parent.ToLower(CultureInfo.InvariantCulture)}* // " +
                $"{(Sub1 != null ? $"{(Sub2 == null ? $"***{Sub1.ToLower(CultureInfo.InvariantCulture)}***" : $"*{Sub1.ToLower(CultureInfo.InvariantCulture)}*")}" : "")}" +
                $"{(Sub2 != null ? $" // ***{Sub2.ToLower(CultureInfo.InvariantCulture)}***" : "")}";
            this.Color = Override ?? Colors.EmbedColor;
            this.Footer = new()
            {
                Text = Strings.EmbedStrings.FooterString,
            };
            this.Timestamp = Strings.EmbedStrings.DefaultTimestamp;
        }

        internal void WriteField(string Name, string Value)
        {
            _ = this.AddField(Strings.EmbedStrings.Empty, $"// *{Name.ToLower(CultureInfo.InvariantCulture)}*\n{Value.ToLower(CultureInfo.InvariantCulture)}");
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
