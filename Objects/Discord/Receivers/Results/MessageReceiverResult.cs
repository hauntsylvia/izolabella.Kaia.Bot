using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Receivers.Results
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
