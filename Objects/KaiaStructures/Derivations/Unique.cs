using izolabella.Storage.Objects.DataStores;
using izolabella.Storage.Objects.Structures;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.KaiaStructures.Derivations
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class Unique : IDataStoreEntity
    {
        public Unique(DataStore BelongsTo, ulong? Id = null)
        {
            this.BelongsTo = BelongsTo;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        public DataStore BelongsTo { get; set; }

        [JsonProperty("Id", Required = Required.Always)]
        public ulong Id { get; }

        public async Task SaveAsync()
        {
            await this.BelongsTo.SaveAsync(this);
        }

        public async Task<T?> GetAsync<T>() where T : class, IDataStoreEntity
        {
            T? R = await this.BelongsTo.ReadAsync<T>(this.Id);
            return R;
        }
    }
}
