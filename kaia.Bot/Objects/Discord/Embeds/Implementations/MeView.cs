using Discord;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations
{
    internal class MeView : CCBEmbed
    {
        public MeView(string UserName, CCB_Structures.CCBUser User) : base()
        {
            this.Description = $"// ***{UserName.ToLower()}***";
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *highest number counted*\n `{User.Settings.HighestCountEver ?? 0}`",
            });
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *total numbers counted*\n `{User.Settings.NumbersCounted ?? 0}`",
            });
        }
    }
}
