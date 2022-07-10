using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds.KaiaServer
{
    public class VerifyEmbed : KaiaPathEmbed
    {
        public VerifyEmbed() : base(Strings.EmbedStrings.FakePaths.Kaia, "verification")
        {
            WithImage(new("https://i.pinimg.com/originals/eb/1a/db/eb1adbec173091c225ad27e8ca55f0ac.gif"));
            WithListWrittenToField("verification", new List<string>()
            {
                $"{Emotes.Customs.KaiaWelcome} : react for access!",
            }, "\n");
        }
    }
}
