using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Message_Receivers.Results
{
    public class MessageReceiverResult
    {
        public MessageReceiverResult(ICCBInventoryItem? ItemToUse = null)
        {
            this.ItemToUse = ItemToUse;
        }

        public ICCBInventoryItem? ItemToUse { get; }
    }
}
