using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases
{
    public abstract class Spell(string Name, string Description, bool SingleUse, KaiaEmote Emote, SpellId Id)
    {
        public string Name { get; } = Name;

        public string Description { get; } = Description;

        public bool SingleUse { get; } = SingleUse;

        public KaiaEmote Emote { get; } = Emote;

        public SpellId Id { get; } = Id;

        public virtual Task ApplyAsync(SpellsProcessor From, KaiaUser ApplyTo)
        {
            return Task.CompletedTask;
        }
    }
}