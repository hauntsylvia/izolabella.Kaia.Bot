using izolabella.KawaiiRed.NET;
using izolabella.KawaiiRed.NET.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Intimates
{
    public class IntimateDisplay : KaiaPathEmbed
    {
        public IntimateDisplay(GenericEndpointType TypeOfInteraction, KaiaUser UserActing, KaiaUser TargetUser) : base(TypeOfInteraction.Singular)
        {
            GenericResponse? Response = new KawaiiRedClient().GetGifAsync(TypeOfInteraction).Result;
            this.WithField(Strings.EmbedStrings.Empty, $"**<@{UserActing.Id}>** {TypeOfInteraction.Verb} **<@{TargetUser.Id}>**");
            if (Response != null)
            {
                this.WithImage(Response.ResponseUrl);
            }
        }
    }
}
