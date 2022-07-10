using izolabella.Kaia.Bot.Objects.Constants;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public class KaiaPathEmbed
    {
        public KaiaPathEmbed(string Parent, string? Sub1 = null, string? Sub2 = null, Color? Override = null, DateTimeOffset? OverrideDate = null)
        {
            this.Parent = Parent;
            this.Sub1 = Sub1;
            this.Sub2 = Sub2;
            this.Override = Override;
            this.OverrideDate = OverrideDate;
            Populate();
        }

        private EmbedBuilder Inner { get; set; } = new();

        public string Parent { get; }

        public string? Sub1 { get; }

        public string? Sub2 { get; }

        public Color? Override { get; }

        public DateTimeOffset? OverrideDate { get; }

        #region upper level customizations

        public KaiaPathEmbed WithField(string Name, string Value, bool OverrideCaps = false)
        {
            Inner.AddField(Strings.EmbedStrings.Empty, $"{(Name == Strings.EmbedStrings.Empty || Name == string.Empty ? string.Empty : $"// *{Name.ToLower(CultureInfo.InvariantCulture)}*\n")}{(OverrideCaps ? Value : Value.ToLower(CultureInfo.InvariantCulture))}");
            return this;
        }

        public KaiaPathEmbed WithListWrittenToField(string Name, IEnumerable<string> Values, string DelimBy = ", ")
        {
            string S = Strings.EmbedStrings.Empty;
            for (int Index = 0; Index < Values.Count(); Index++)
            {
                S += $"{(Index != 0 ? DelimBy : string.Empty)}{Values.ElementAt(Index)}";
            }
            WithField(Name, S);
            return this;
        }

        public KaiaPathEmbed WithImage(Uri Url)
        {
            Inner.WithImageUrl(Url.ToString());
            return this;
        }

        #endregion

        public KaiaPathEmbed MakeNaked()
        {
            Inner = new();
            return this;
        }

        private KaiaPathEmbed Populate()
        {
            string Parent = this.Parent.ToLower(CultureInfo.InvariantCulture);
            Inner.Description =
                $"" +
                $"{(Sub1 != null ? $"*{Parent}*" : $"***{Parent}***")}{(Sub1 == null ? "" : " // ")}" +
                $"{(Sub1 != null ? $"{(Sub2 == null ? $"***{Sub1.ToLower(CultureInfo.InvariantCulture)}***" : $"*{Sub1.ToLower(CultureInfo.InvariantCulture)}*")}" : "")}" +
                $"{(Sub2 != null ? $" // ***{Sub2.ToLower(CultureInfo.InvariantCulture)}***" : "")}";
            Inner.Color = Override ?? Colors.EmbedColor;
            Inner.Footer = new()
            {
                Text = Strings.EmbedStrings.FooterString,
            };
            Inner.Timestamp = OverrideDate ?? Strings.EmbedStrings.DefaultTimestamp;
            return this;
        }

        public Embed Build()
        {
            Embed Pre = Inner.Build();
            MakeNaked();
            Populate();
            return Pre;
        }
    }
}
