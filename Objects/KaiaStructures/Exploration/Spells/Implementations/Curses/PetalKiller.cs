using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Implementations.Curses
{
    public class PetalKiller : Spell
    {
        public PetalKiller() : base($"{Strings.Economy.CurrencyName} Killer",
                                    $"Halves your {Strings.Economy.CurrencyName}.",
                                    true,
                                    Emotes.Counting.Curses,
                                    new(DateTime.UtcNow, 7120222142))
        {
        }

        public override Task ApplyAsync(SpellsProcessor From, KaiaUser ApplyTo)
        {
            ApplyTo.Settings.Inventory.Petals /= 2;
            return Task.CompletedTask;
        }
    }
}
