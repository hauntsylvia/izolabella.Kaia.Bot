using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;

namespace Kaia.Bot.Objects.Discord.Message_Receivers.Results
{
    public class MessageReceiverResult
    {
        public MessageReceiverResult(bool SaveUser = true, bool SaveUserGuild = true, IKaiaInventoryItem? ItemToUse = null)
        {
            this.SaveUser = SaveUser;
            this.SaveUserGuild = SaveUserGuild;
            this.ItemToUse = ItemToUse;
        }

        public bool SaveUser { get; }
        public bool SaveUserGuild { get; }
        public IKaiaInventoryItem? ItemToUse { get; set; }
    }
}
