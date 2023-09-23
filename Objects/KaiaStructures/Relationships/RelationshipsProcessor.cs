using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Storage.Objects.DataStores;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships
{
    public class RelationshipsProcessor
    {
        public RelationshipsProcessor(ulong U)
        {
            this.U = U;
        }

        public ulong U { get; }

        private DataStore RelationshipsStore { get; } = DataStores.UserRelationshipsMainDirectory;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>An <see cref="IEnumerable{UserRelationship}"/> of the relationships this user is involved with.</returns>
        public async Task<IEnumerable<UserRelationship>> GetRelationshipsAsync()
        {
            return (await this.RelationshipsStore.ReadAllAsync<UserRelationship>())
                                                 .Where(R => R.KaiaUserIds.Any(U => this.U == U));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>An <see cref="IEnumerable{UserRelationship}"/> of the relationships this user is involved with.</returns>
        public async Task<IEnumerable<UserRelationship>> GetPendingRelationshipsAsync()
        {
            return (await this.RelationshipsStore.ReadAllAsync<UserRelationship>())
                                                 .Where(R => R.PendingIds.Any(U => this.U == U));
        }
    }
}