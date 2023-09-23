using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Receivers.Results
{
    public class ReceiverResult(KaiaUser? UserToSave = null, KaiaGuild? GuildToSave = null, KaiaInventoryItem? ItemToUse = null)
    {
        public KaiaUser? UserToSave { get; set; } = UserToSave;

        public KaiaGuild? GuildToSave { get; set; } = GuildToSave;

        public KaiaInventoryItem? ItemToUse { get; set; } = ItemToUse;
    }
}