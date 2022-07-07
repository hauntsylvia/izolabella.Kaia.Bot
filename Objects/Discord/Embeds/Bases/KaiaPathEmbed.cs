namespace Kaia.Bot.Objects.Discord.Embeds.Bases
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
            this.Populate();
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
            this.Inner.AddField(Strings.EmbedStrings.Empty, $"// *{Name.ToLower(CultureInfo.InvariantCulture)}*\n{(OverrideCaps ? Value : Value.ToLower(CultureInfo.InvariantCulture))}");
            return this;
        }

        public KaiaPathEmbed WithListWrittenToField(string Name, IEnumerable<string> Values, string DelimBy = ", ")
        {
            string S = Strings.EmbedStrings.Empty;
            for (int Index = 0; Index < Values.Count(); Index++)
            {
                S += $"{(Index != 0 ? DelimBy : string.Empty)}{Values.ElementAt(Index)}";
            }
            this.WithField(Name, S);
            return this;
        }

        public KaiaPathEmbed WithImage(Uri Url)
        {
            this.Inner.WithImageUrl(Url.ToString());
            return this;
        }

        #endregion

        public KaiaPathEmbed MakeNaked()
        {
            this.Inner = new();
            return this;
        }

        private KaiaPathEmbed Populate()
        {
            string Parent = this.Parent.ToLower(CultureInfo.InvariantCulture);
            this.Inner.Description =
                $"" +
                $"{(this.Sub1 != null ? $"*{Parent}*" : $"***{Parent}***")}{(this.Sub1 == null ? "" : " // ")}" +
                $"{(this.Sub1 != null ? $"{(this.Sub2 == null ? $"***{this.Sub1.ToLower(CultureInfo.InvariantCulture)}***" : $"*{this.Sub1.ToLower(CultureInfo.InvariantCulture)}*")}" : "")}" +
                $"{(this.Sub2 != null ? $" // ***{this.Sub2.ToLower(CultureInfo.InvariantCulture)}***" : "")}";
            this.Inner.Color = this.Override ?? Colors.EmbedColor;
            this.Inner.Footer = new()
            {
                Text = Strings.EmbedStrings.FooterString,
            };
            this.Inner.Timestamp = this.OverrideDate ?? Strings.EmbedStrings.DefaultTimestamp;
            return this;
        }

        public Embed Build()
        {
            Embed Pre = this.Inner.Build();
            this.MakeNaked();
            this.Populate();
            return Pre;
        }
    }
}
