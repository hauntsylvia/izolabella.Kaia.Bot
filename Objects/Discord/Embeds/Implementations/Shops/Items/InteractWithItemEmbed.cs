namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class InteractWithItemEmbed : KaiaPathEmbed
    {
        public InteractWithItemEmbed(KaiaInventoryItem I, KaiaUser U) : base(Strings.EmbedStrings.FakePaths.Inventory, $"{I.DisplayName} [interaction]")
        {
            U.Settings.Inventory.RemoveItemOfIdAsync(I).Wait();
            U.SaveAsync().Wait();
            if(I.OnInteract != null)
            {
                if(I.OnInteract.Message.Length > 0)
                {
                    this.WithField("?", $"```{I.OnInteract.Message}```");
                }
                if (I.OnInteract.Reward != null)
                {
                    if (I.OnInteract.Reward.Petals is > 0 or < 0)
                    {
                        U.Settings.Inventory.Petals += I.OnInteract.Reward.Petals;
                        this.WithField($"{Strings.Economy.CurrencyName}", $"{Strings.Economy.CurrencyEmote} {(I.OnInteract.Reward.Petals > 0 ? "+" : "-")}`{Math.Abs(I.OnInteract.Reward.Petals)}`");
                    }
                    if (I.OnInteract.Reward.Items.Length > 0)
                    {
                        U.Settings.Inventory.AddItemsToInventoryAndSaveAsync(U, I.OnInteract.Reward.Items).Wait();
                        List<string> Display = new();
                        foreach (KaiaInventoryItem Item in I.OnInteract.Reward.Items)
                        {
                            Display.Add($"[worth {Strings.Economy.CurrencyEmote} `{Item.MarketCost}`] {Item.DisplayName} {Item.DisplayEmote}");
                        }
                        this.WithListWrittenToField("items found", Display, ",\n");
                    }
                    if (I.OnInteract.Reward.Spells.Any())
                    {
                        List<string> Display = new();
                        foreach (Spell Spell in I.OnInteract.Reward.Spells)
                        {
                            U.SpellsProcessor.ApplySpellAndSaveAsync(U, Spell).Wait();
                            Display.Add($"__{Spell.Name} {Spell.Emote}__ {(!Spell.SingleUse ? $"[until (utc) `{Spell.Id.ActiveUntil.ToUniversalTime().ToShortDateString()}` @ `{Spell.Id.ActiveUntil.ToUniversalTime().ToShortTimeString()}`]" : "")} ```{Spell.Description}```");
                        }
                        this.WithListWrittenToField("spells applied", Display, "\n");
                    }
                }
            }
        }
    }
}
