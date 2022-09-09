using izolabella.Storage.Objects.DataStores;
using izolabella.Storage.Objects.Structures;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Derivations;

[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
public class Unique : IDataStoreEntity
{
    public Unique(DataStore? BelongsTo, ulong? Id = null)
    {
        this.BelongsTo = BelongsTo;
        this.Id = Id ?? IdGenerator.CreateNewId();
    }

    public DataStore? BelongsTo { get; set; }

    [JsonProperty("Id", Required = Required.Always)]
    public ulong Id { get; }

    public Task SaveAsync()
    {
        if (this.BelongsTo != null)
        {
            lock (this)
            {
                this.BelongsTo.SaveAsync(this).Wait();
            }
        }
        return Task.CompletedTask;
    }

    public async Task<T?> GetAsync<T>() where T : class, IDataStoreEntity
    {
        T? R = this.BelongsTo != null ? await this.BelongsTo.ReadAsync<T>(this.Id) : null;
        return R;
    }
}
