namespace Kaia.Bot.Objects.Discord.MessageReceivers.Results
{
    public class MessageReceiverResult
    {
        public MessageReceiverResult(bool SaveUser = true, bool SaveUserGuild = true, KaiaInventoryItem? ItemToUse = null)
        {
            this.SaveUser = SaveUser;
            this.SaveUserGuild = SaveUserGuild;
            this.ItemToUse = ItemToUse;
        }

        public bool SaveUser { get; }
        public bool SaveUserGuild { get; }
        public KaiaInventoryItem? ItemToUse { get; set; }
    }
}
