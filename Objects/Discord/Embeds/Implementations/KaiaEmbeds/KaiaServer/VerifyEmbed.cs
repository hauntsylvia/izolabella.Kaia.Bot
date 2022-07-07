using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds.KaiaServer
{
    public class VerifyEmbed : KaiaPathEmbed
    {
        public VerifyEmbed() : base(Strings.EmbedStrings.FakePaths.Kaia, "verification")
        {
            this.WithImage(new("https://i.pinimg.com/originals/eb/1a/db/eb1adbec173091c225ad27e8ca55f0ac.gif"));
            this.WithListWrittenToField("verification", new List<string>()
            {
                $"{Emotes.Customs.KaiaWelcome} : react for access!",
            }, "\n");
        }
    }
}
