using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Implementations.Blessings;

public class ExhaustionRevival : Spell
{
    public ExhaustionRevival() : base("Exhaustion Removal",
                                      "Resets the timeouts for all locations.",
                                      true,
                                      Emotes.Counting.Blessings,
                                      new(TimeSpan.FromHours(4), 7120221947))
    {

    }

    public override async Task ApplyAsync(SpellsProcessor From, KaiaUser ApplyTo)
    {
        await ApplyTo.LocationProcessor.RemoveAllLocationsExploredAsync();
    }
}
