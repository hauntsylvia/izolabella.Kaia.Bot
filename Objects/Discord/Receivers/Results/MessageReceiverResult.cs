namespace Kaia.Bot.Objects.Discord.Receivers.Results
{
    public class ReceiverResult
    {
        public ReceiverResult(KaiaUser? UserToSave = null, KaiaGuild? GuildToSave = null, KaiaInventoryItem? ItemToUse = null)
        {
            this.UserToSave = UserToSave;
            this.GuildToSave = GuildToSave;
            this.ItemToUse = ItemToUse;
        }

        public KaiaUser? UserToSave { get; set; }

        public KaiaGuild? GuildToSave { get; set; }

        public KaiaInventoryItem? ItemToUse { get; set; }
    }
}
