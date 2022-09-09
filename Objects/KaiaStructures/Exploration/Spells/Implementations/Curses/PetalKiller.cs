using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Implementations.Curses;

public class PetalKiller : Spell
{
    public PetalKiller() : base($"{Strings.Economy.CurrencyName} Killer",
                                $"Halves your {Strings.Economy.CurrencyName}.",
                                true,
                                Emotes.Counting.Curses,
                                new(TimeSpan.FromHours(4), 7120222142))
    {
    }

    public override Task ApplyAsync(SpellsProcessor From, KaiaUser ApplyTo)
    {
        ApplyTo.Settings.Inventory.Petals /= 2;
        return Task.CompletedTask;
    }
}
