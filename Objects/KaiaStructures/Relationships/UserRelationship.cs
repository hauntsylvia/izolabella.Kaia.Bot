using izolabella.Storage.Objects.Structures;
using izolabella.Util;

namespace Kaia.Bot.Objects.KaiaStructures.Relationships
{
    public class UserRelationship : IDataStoreEntity
    {
        public UserRelationship(DateTime CreatedAt, string Description, KaiaEmote Emote, Dictionary<ulong, DateTime> PendingIds, Dictionary<ulong, DateTime> KaiaUserIds, ulong? Id = null)
        {
            this.CreatedAt = CreatedAt;
            this.description = Description.Length > 512 ? Description[..512] : Description;
            this.Emote = Emote;
            this.kaiaUserIds = KaiaUserIds;
            this.pendingIds = PendingIds;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        public DateTime CreatedAt { get; }

        [JsonProperty("Description")]
        private readonly string description;

        [JsonIgnore]
        public string Description => this.description.Length > 32 ? this.description[..32] : this.description;

        public KaiaEmote Emote { get; }

        public ulong Id { get; }

        [JsonProperty("KaiaUserIds")]
        private readonly Dictionary<ulong, DateTime> kaiaUserIds;

        [JsonIgnore]
        public IEnumerable<ulong> KaiaUserIds => this.kaiaUserIds.Keys.Distinct().ToArray();

        [JsonProperty("PendingUserIds")]
        private readonly Dictionary<ulong, DateTime> pendingIds;

        [JsonIgnore]
        public IEnumerable<ulong> PendingIds => this.pendingIds.Keys.Distinct();

        [JsonIgnore]
        public int NumberOfMembers => this.KaiaUserIds.Count();

        public void AddPendingMember(ulong Member)
        {
            this.pendingIds.Add(Member, DateTime.UtcNow);
        }

        public void AddMember(ulong Member)
        {
            this.UserDeclines(Member);
            if(!this.kaiaUserIds.ContainsKey(Member))
            {
                this.kaiaUserIds.Add(Member, DateTime.UtcNow);
            }
        }

        public void UserDeclines(ulong UserThatDeclined)
        {
            if(this.pendingIds.ContainsKey(UserThatDeclined))
            {
                this.pendingIds.Remove(UserThatDeclined);
                this.UserDeclines(UserThatDeclined);
            }
            else
            {
                return;
            }
        }
    }
}
