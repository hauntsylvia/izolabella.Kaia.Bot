using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                this.UserLocationStore = DataStores.GetUserAchievementStore(this.U);
            }
        }

        public ulong U { get; }

        private DataStore? UserLocationStore { get; }

        public async Task<IReadOnlyCollection<KaiaLocation>> GetUserLocationsExploredAsync()
        {
            return await (this.UserLocationStore != null ? this.UserLocationStore.ReadAllAsync<KaiaLocation>() : Task.FromResult(new List<KaiaLocation>()));
        }
    }
}
