﻿using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties
{
    public class KaiaSpellsRoom
    {
        public static List<Spell> Spells => BaseImplementationUtil.GetItems<Spell>();

        public static IEnumerable<Spell> GetSpellsFromIds(params SpellId[] SpellIds)
        {
            IEnumerable<Spell> Spells = KaiaSpellsRoom.Spells.Where(KaiaSpell => SpellIds.Any(SpellId => SpellId.Id == KaiaSpell.Id.Id));
            return Spells;
        }
    }
}