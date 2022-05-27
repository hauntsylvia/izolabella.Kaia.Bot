using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Interfaces;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    internal class StoreTransactionCompleted : KaiaPathEmbed
    {
        internal StoreTransactionCompleted(KaiaUser User, List<IKaiaInventoryItem> ItemsBuying) : base(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            this.WriteListToOneField("customer", new()
            {
                $"current {Strings.Economy.CurrencyName} {Strings.Economy.CurrencyEmote}: `{User.Settings.Inventory.Petals}`"
            }, "\n");
            List<string> Display = new();
            foreach (IKaiaInventoryItem Item in ItemsBuying)
            {
                Display.Add($"{Item.DisplayName} :: {Strings.Economy.CurrencyName} {Strings.Economy.CurrencyEmote} `{Item.Cost}`");
            }
            this.WriteListToOneField($"{(ItemsBuying.Count == 1 ? "item bought" : "items bought")}", Display, "\n");
        }
    }
}
