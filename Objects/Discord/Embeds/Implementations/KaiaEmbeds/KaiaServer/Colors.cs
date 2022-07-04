using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds.KaiaServer
{
    public class ColorsEmbed : KaiaPathEmbed
    {
        public ColorsEmbed() : base(Strings.EmbedStrings.FakePaths.Kaia, "colors")
        {
            this.WithImage(new("https://i.pinimg.com/originals/33/4e/a1/334ea17c92dfccd6418b3ebe9206aaa7.gif"));
            this.WithListWrittenToField("colors", new()
            {
                $"{Emotes.Customs.Numbers.Kaia1} : <@&993547948254318722>",
                $"{Emotes.Customs.Numbers.Kaia2} : <@&993548551839830096>",
                $"{Emotes.Customs.Numbers.Kaia3} : <@&993547755052081282>",
                $"{Emotes.Customs.Numbers.Kaia4} : <@&993548654210199583>",
                $"{Emotes.Customs.Numbers.Kaia5} : <@&993548341399015634>",
                $"{Emotes.Customs.Numbers.Kaia6} : <@&993548121508421742>",
                $"{Emotes.Customs.Numbers.Kaia7} : <@&993548486597423195>",
                $"{Emotes.Customs.Numbers.Kaia8} : <@&993547585098874901>",
                $"{Emotes.Customs.Numbers.Kaia9} : <@&993547862346567830>",
            }, "\n");
        }
    }
}
