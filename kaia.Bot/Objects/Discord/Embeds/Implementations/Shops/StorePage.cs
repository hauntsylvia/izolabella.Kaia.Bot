using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class StorePage : CCBPathEmbed
    {
        public StorePage(string UserName, CCB_Structures.CCBUser User, List<ICCBInventoryItem> Display) : base(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            foreach (ICCBInventoryItem Item in Display)
            {
                this.WriteField($"[{Strings.Economy.CurrencyEmote} `{Item.Cost}`] {Item.DisplayName}  {Item.DisplayEmote}", Item.Description);
            }
        }
    }
}
