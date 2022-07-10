using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using izolabella.Storage.Objects.DataStores;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserLocationRoom
    {
        public UserLocationRoom(ulong U)
        {
            this.U = U;
            if (this.U != default)
            {
                UserLocationStore = DataStores.GetUserLocationsStore(this.U);
            }
        }

        public ulong U { get; }

        private DataStore? UserLocationStore { get; }

        public async Task<IEnumerable<KaiaLocation>> GetUserLocationsExploredAsync()
        {
            List<KaiaLocation> Locs = await (UserLocationStore != null ? UserLocationStore.ReadAllAsync<KaiaLocation>() : Task.FromResult(new List<KaiaLocation>()));
            if (UserLocationStore != null)
            {
                foreach (KaiaLocation Loc in Locs.Where(L => L.MustWaitUntil < DateTime.UtcNow))
                {
                    await UserLocationStore.DeleteAsync(Loc.Id);
                }
            }
            return Locs.Where(L => L.MustWaitUntil >= DateTime.UtcNow);
        }

        public async Task AddLocationExploredAsync(KaiaLocation L)
        {
            await (UserLocationStore != null ? UserLocationStore.SaveAsync(L) : Task.CompletedTask);
        }

        public async Task RemoveAllLocationsExploredAsync()
        {
            if (UserLocationStore != null)
            {
                await UserLocationStore.DeleteAllAsync();
            }
        }
    }
}
