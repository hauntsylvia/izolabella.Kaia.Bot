using izolabella.Storage.Objects.Structures;
using izolabella.Util;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases
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
