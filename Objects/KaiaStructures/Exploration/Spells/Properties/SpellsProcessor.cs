using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Storage.Objects.DataStores;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties
{
    public class SpellsProcessor
    {
        public SpellsProcessor(ulong U)
        {
            this.U = U;
            if (U != default)
            {
                SpellsStore = DataStores.GetUserSpellsStore(this.U);
            }
        }

        private ulong U { get; }

        private DataStore? SpellsStore { get; }

        public async Task ApplySpellAndSaveAsync(KaiaUser UserToApplyTo, Spell SpellToApply)
        {
            if (SpellsStore != null)
            {
                await SpellToApply.ApplyAsync(this, UserToApplyTo);
                if (!SpellToApply.SingleUse)
                {
                    await SpellsStore.SaveAsync(SpellToApply.Id);
                }
            }
        }

        public async Task<IEnumerable<Spell>> GetActiveSpellsAsync()
        {
            List<SpellId> UserSpells = await (SpellsStore != null ? SpellsStore.ReadAllAsync<SpellId>() : Task.FromResult<List<SpellId>>(new()));
            if (SpellsStore != null)
            {
                foreach (SpellId Inactive in UserSpells.Where(A => DateTime.UtcNow > A.ActiveUntil))
                {
                    await SpellsStore.DeleteAsync(Inactive.Id);
                }
            }
            return KaiaSpellsRoom.Spells.Where(KaiaSpell => UserSpells.Where(UserSpell => DateTime.UtcNow <= UserSpell.ActiveUntil).Any(UserSpell => UserSpell.Id == KaiaSpell.Id.Id));
        }
    }
}
