using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties
{
    public class SpellsProcessor
    {
        public SpellsProcessor(ulong U)
        {
            this.U = U;
            if (U != default)
            {
                this.SpellsStore = DataStores.GetUserSpellsStore(this.U);
            }
        }

        private ulong U { get; }

        private DataStore? SpellsStore { get; }

        public async Task ApplySpellAndSaveAsync(KaiaUser UserToApplyTo, Spell SpellToApply)
        {
            if(this.SpellsStore != null)
            {
                await SpellToApply.ApplyAsync(this, UserToApplyTo);
                if (!SpellToApply.SingleUse)
                {
                    await this.SpellsStore.SaveAsync(SpellToApply.Id);
                }
            }
        }

        public async Task<IEnumerable<Spell>> GetActiveSpellsAsync()
        {
            List<SpellId> UserSpells = await (this.SpellsStore != null ? this.SpellsStore.ReadAllAsync<SpellId>() : Task.FromResult<List<SpellId>>(new()));
            if(this.SpellsStore != null)
            {
                foreach (SpellId Inactive in UserSpells.Where(A => DateTime.UtcNow > A.ActiveUntil))
                {
                    await this.SpellsStore.DeleteAsync(Inactive.Id);
                }
            }
            return KaiaSpellsRoom.Spells.Where(KaiaSpell => UserSpells.Where(UserSpell => DateTime.UtcNow <= UserSpell.ActiveUntil).Any(UserSpell => UserSpell.Id == KaiaSpell.Id.Id));
        }
    }
}
