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
        public SpellId(DateTime ActiveUntil, ulong Id)
        {
            this.ActiveUntil = ActiveUntil;
            this.Id = Id;
        }

        public DateTime ActiveUntil { get; }

        public ulong Id { get; }
    }
}
