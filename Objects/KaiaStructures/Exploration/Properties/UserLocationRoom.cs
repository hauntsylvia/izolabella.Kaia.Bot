using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserLocationRoom
    {
        public UserLocationRoom(ulong U)
        {
            this.U = U;
            if (this.U != default)
            {
                this.UserLocationStore = DataStores.GetUserLocationsStore(this.U);
            }
        }

        public ulong U { get; }

        private DataStore? UserLocationStore { get; }

        public async Task<IEnumerable<KaiaLocation>> GetUserLocationsExploredAsync()
        {
            List<KaiaLocation> Locs = await (this.UserLocationStore != null ? this.UserLocationStore.ReadAllAsync<KaiaLocation>() : Task.FromResult(new List<KaiaLocation>()));
            if(this.UserLocationStore != null)
            {
                foreach (KaiaLocation Loc in Locs.Where(L => L.MustWaitUntil < DateTime.UtcNow))
                {
                    await this.UserLocationStore.DeleteAsync(Loc.Id);
                }
            }
            return Locs.Where(L => L.MustWaitUntil >= DateTime.UtcNow);
        }

        public async Task AddLocationExploredAsync(KaiaLocation L)
        {
            await ( this.UserLocationStore != null ? this.UserLocationStore.SaveAsync(L) : Task.CompletedTask );
        }

        public async Task RemoveAllLocationsExploredAsync()
        {
            if(this.UserLocationStore != null)
            {
                await this.UserLocationStore.DeleteAllAsync();
            }
        }
    }
}
