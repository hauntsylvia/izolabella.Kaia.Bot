using Discord;
using Kaia.Bot.Objects.Constants;
using Kaia.Bot.Objects.Discord.Embeds.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    internal class MeView : CCBPathEmbed
    {
        public MeView(string UserName, CCB_Structures.CCBUser User) : base(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Users, UserName)
        {
            this.WriteField("highest number counted", $"`{User.Settings.HighestCountEver ?? 0}`");
            this.WriteField("total numbers counted", $"`{User.Settings.NumbersCounted ?? 0}`");
            this.WriteField($"{Strings.Economy.CurrencyEmote} current {Strings.Economy.CurrencyName}", $"`{User.Settings.Inventory.Petals}`");
        }
    }
}
