using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Constants.Embeds
{
    internal class EmbedDefaults
    {
        public static KaiaPathEmbed DefaultEmbedForNoItemsPresent => new KaiaPathEmbed(Strings.EmbedStrings.FakePaths.Global, "?").WithField("?", "no items found!").WithField("204", "yup. u just got an http code from a discord bot.");
        public static KaiaPathEmbed RateLimitEmbed => new KaiaPathEmbed(Strings.EmbedStrings.FakePaths.Global, "rate limited").WithField("slow down", "wait.");
    }
}
