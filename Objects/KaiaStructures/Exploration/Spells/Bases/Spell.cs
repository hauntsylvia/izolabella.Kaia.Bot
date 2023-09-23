using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases
{
    public abstract class Spell
    {
        public Spell(string Name, string Description, bool SingleUse, KaiaEmote Emote, SpellId Id)
        {
            this.Name = Name;
            this.Description = Description;
            this.SingleUse = SingleUse;
            this.Emote = Emote;
            this.Id = Id;
        }

        public string Name { get; }

        public string Description { get; }

        public bool SingleUse { get; }

        public KaiaEmote Emote { get; }

        public SpellId Id { get; }

        public virtual Task ApplyAsync(SpellsProcessor From, KaiaUser ApplyTo)
        {
            return Task.CompletedTask;
        }
    }
}