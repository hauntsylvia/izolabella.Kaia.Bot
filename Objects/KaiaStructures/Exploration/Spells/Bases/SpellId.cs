using izolabella.Storage.Objects.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases
{
    public class SpellId : IDataStoreEntity
    {
        public SpellId(TimeSpan ActiveFor, ulong Id)
        {
            this.ActiveUntil = DateTime.UtcNow.Add(ActiveFor);
            this.Id = Id;
        }

        [JsonConstructor]
        public SpellId(DateTime ActiveUntil, ulong Id)
        {
            this.ActiveUntil = ActiveUntil;
            this.Id = Id;
        }

        public DateTime ActiveUntil { get; }

        public bool Expired => DateTime.UtcNow > this.ActiveUntil;

        public ulong Id { get; }
    }
}
