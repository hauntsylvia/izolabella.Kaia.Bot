using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public class KaiaPathEmbed
    {
        public KaiaPathEmbed(string Parent, string? Sub1 = null, string? Sub2 = null, Color? Override = null)
        {
            this.Parent = Parent;
            this.Sub1 = Sub1;
            this.Sub2 = Sub2;
            this.Override = Override;
            this.Populate();
        }

        private EmbedBuilder Inner { get; set; } = new();

        public string Parent { get; }

        public string? Sub1 { get; }

        public string? Sub2 { get; }

        public Color? Override { get; }

        public KaiaPathEmbed WithField(string Name, string Value)
        {
            this.Inner.AddField(Strings.EmbedStrings.Empty, $"// *{Name.ToLower(CultureInfo.InvariantCulture)}*\n{Value.ToLower(CultureInfo.InvariantCulture)}");
            return this;
        }

        public KaiaPathEmbed WithListWrittenToField(string Name, List<string> Values, string DelimBy = ", ")
        {
            string S = Strings.EmbedStrings.Empty;
            for (int Index = 0; Index < Values.Count; Index++)
            {
                S += $"{(Index != 0 ? DelimBy : string.Empty)}{Values[Index]}";
            }
            this.WithField(Name, S);
            return this;
        }

        public KaiaPathEmbed MakeNaked()
        {
            this.Inner = new();
            return this;
        }

        private KaiaPathEmbed Populate()
        {
            this.Inner.Description =
                $"" +
                $"*{this.Parent.ToLower(CultureInfo.InvariantCulture)}*{(this.Sub1 == null ? "" : " // ")}" +
                $"{(this.Sub1 != null ? $"{(this.Sub2 == null ? $"***{this.Sub1.ToLower(CultureInfo.InvariantCulture)}***" : $"*{this.Sub1.ToLower(CultureInfo.InvariantCulture)}*")}" : "")}" +
                $"{(this.Sub2 != null ? $" // ***{this.Sub2.ToLower(CultureInfo.InvariantCulture)}***" : "")}";
            this.Inner.Color = this.Override ?? Colors.EmbedColor;
            this.Inner.Footer = new()
            {
                Text = Strings.EmbedStrings.FooterString,
            };
            this.Inner.Timestamp = Strings.EmbedStrings.DefaultTimestamp;
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
