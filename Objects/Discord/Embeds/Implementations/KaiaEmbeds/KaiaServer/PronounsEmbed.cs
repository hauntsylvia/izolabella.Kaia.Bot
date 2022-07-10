using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds.KaiaServer
{
    public class PronounsEmbed : KaiaPathEmbed
    {
        public PronounsEmbed() : base(Strings.EmbedStrings.FakePaths.Kaia, "pronouns")
        {
            this.WithImage(new("https://i.pinimg.com/originals/1d/89/6a/1d896ab3d33457c4b50befc3a2a342b9.gif"));
            this.WithListWrittenToField("pronouns", new List<string>()
            {
                $"{Emotes.Customs.Numbers.Kaia1} : <@&993246110997037117>",
                $"{Emotes.Customs.Numbers.Kaia2} : <@&993247472086433802>",
                $"{Emotes.Customs.Numbers.Kaia3} : <@&993247285653798992>",
                $"{Emotes.Customs.Numbers.Kaia4} : <@&993552846677090376>",
            }, "\n");
        }
    }
}
