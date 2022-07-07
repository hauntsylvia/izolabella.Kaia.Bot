using izolabella.Storage.Objects.Structures;
using izolabella.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Relationships
{
    public class UserRelationship : IDataStoreEntity
    {
        public UserRelationship(DateTime CreatedAt, Dictionary<ulong, DateTime> PendingIds, Dictionary<ulong, DateTime> KaiaUserIds, ulong? Id = null)
        {
            this.CreatedAt = CreatedAt;
            this.kaiaUserIds = KaiaUserIds;
            this.pendingIds = PendingIds;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        public DateTime CreatedAt { get; }

        public ulong Id { get; }

        private readonly Dictionary<ulong, DateTime> kaiaUserIds;

        public IEnumerable<ulong> KaiaUserIds => this.kaiaUserIds.Keys.Distinct();

        private readonly Dictionary<ulong, DateTime> pendingIds;

        public IEnumerable<ulong> PendingIds => this.pendingIds.Keys.Distinct();

        [JsonIgnore]
        public int NumberOfMembers => this.KaiaUserIds.Count();

        public void AddPendingMember(ulong Member)
        {
            this.pendingIds.Add(Member, DateTime.UtcNow);
        }

        public void AddMember(ulong Member)
        {
            this.kaiaUserIds.Add(Member, DateTime.UtcNow);
        }
    }
}
