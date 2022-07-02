using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Implementations.Blessings
{
    public class ExhaustionRevival : Spell
    {
        public ExhaustionRevival() : base("Exhaustion Removal",
                                          "Resets the timeouts for all locations.",
                                          true,
                                          Emotes.Counting.Blessings,
                                          new(DateTime.UtcNow, 7120221947))
        {

        }

        public override async Task ApplyAsync(SpellsProcessor From, KaiaUser ApplyTo)
        {
            await ApplyTo.LocationProcessor.RemoveAllLocationsExploredAsync();
        }
    }
}
