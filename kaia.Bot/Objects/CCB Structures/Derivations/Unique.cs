using izolabella.Storage.Objects.DataStores;
using izolabella.Storage.Objects.Structures;
using Kaia.Bot.Objects.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Derivations
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class Unique : IDataStoreEntity
    {
        public Unique(DataStore BelongsTo, ulong? Id = null)
        {
            this.BelongsTo = BelongsTo;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        public DataStore BelongsTo { get; }

        [JsonProperty("Id", Required = Required.Always)]
        public ulong Id { get; }

        public async Task SaveAsync()
        {
            await this.BelongsTo.SaveAsync(this);
        }

        public async Task<T?> GetAsync<T>() where T : IDataStoreEntity
        {
            T? R = await this.BelongsTo.ReadAsync<T>(this.Id);
            return R;
        }
    }
}
