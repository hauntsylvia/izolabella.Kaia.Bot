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
        public Unique(ulong? Id = null)
        {
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        [JsonProperty("Id", Required = Required.Always)]
        public ulong Id { get; }
    }
}
