using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    internal class StoreTransactionCompleted : CCBPathEmbed
    {
        internal StoreTransactionCompleted(CCBUser User) : base(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            this.WriteListToOneField("customer", new()
            {
                $"current {Strings.Economy.CurrencyName} {Strings.Economy.CurrencyEmote}: `{User.Settings.Inventory.Petals}`"
            }, "\n");
        }
    }
}
