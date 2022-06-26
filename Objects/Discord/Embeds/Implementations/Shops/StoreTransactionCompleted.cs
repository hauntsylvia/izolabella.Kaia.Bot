namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    internal class StoreTransactionCompleted : KaiaPathEmbed
    {
        internal StoreTransactionCompleted(KaiaUser User, List<KaiaInventoryItem> ItemsBuying) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop)
        {
            this.WithListWrittenToField("customer", new()
            {
                $"current {Strings.Economy.CurrencyName} {Strings.Economy.CurrencyEmote}: `{User.Settings.Inventory.Petals}`"
            }, "\n");
            List<string> Display = new();
            foreach (KaiaInventoryItem Item in ItemsBuying)
            {
                Display.Add($"{Item.DisplayName} :: {Strings.Economy.CurrencyName} {Strings.Economy.CurrencyEmote} `{Item.Cost}`");
            }
            this.WithListWrittenToField($"{(ItemsBuying.Count == 1 ? "item bought" : "items bought")}", Display, "\n");
        }
    }
}
