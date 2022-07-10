using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Storage.Objects.Structures;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships
{
    public class UserRelationship : IDataStoreEntity
    {
        public UserRelationship(DateTime CreatedAt, string Description, KaiaEmote Emote, Dictionary<ulong, DateTime> PendingIds, Dictionary<ulong, DateTime> KaiaUserIds, ulong? Id = null)
        {
            this.CreatedAt = CreatedAt;
            description = Description.Length > 512 ? Description[..512] : Description;
            this.Emote = Emote;
            kaiaUserIds = KaiaUserIds;
            pendingIds = PendingIds;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        public DateTime CreatedAt { get; }

        [JsonProperty("Description")]
        private readonly string description;

        [JsonIgnore]
        public string Description => description.Length > 32 ? description[..32] : description.Length == 0 ? Strings.EmbedStrings.Empty : description;

        public KaiaEmote Emote { get; }

        public ulong Id { get; }

        [JsonProperty("KaiaUserIds")]
        private readonly Dictionary<ulong, DateTime> kaiaUserIds;

        [JsonIgnore]
        public IEnumerable<ulong> KaiaUserIds => kaiaUserIds.Keys.Distinct().ToArray();

        [JsonProperty("PendingUserIds")]
        private readonly Dictionary<ulong, DateTime> pendingIds;

        [JsonIgnore]
        public IEnumerable<ulong> PendingIds => pendingIds.Where(A => DateTime.UtcNow.Subtract(A.Value).TotalDays < 1).Select(KV => KV.Key).Distinct();

        [JsonIgnore]
        public int NumberOfMembers => KaiaUserIds.Count();

        [JsonIgnore]
        public bool AtMax => KaiaUserIds.Count() >= 50;

        public bool AddPendingMember(ulong Member)
        {
            if (AtMax)
            {
                return false;
            }
            pendingIds.TryAdd(Member, DateTime.UtcNow);
            return true;
        }

        public bool AddMember(ulong Member)
        {
            if (AtMax)
            {
                return false;
            }
            UserDeclines(Member);
            if (!kaiaUserIds.ContainsKey(Member))
            {
                kaiaUserIds.Add(Member, DateTime.UtcNow);
            }
            return true;
        }

        public void RemoveMember(ulong Member)
        {
            UserDeclines(Member);
            if (kaiaUserIds.ContainsKey(Member))
            {
                kaiaUserIds.Remove(Member);
                RemoveMember(Member);
            }
            else
            {
                return;
            }
        }

        public void UserDeclines(ulong UserThatDeclined)
        {
            if (pendingIds.ContainsKey(UserThatDeclined))
            {
                pendingIds.Remove(UserThatDeclined);
                UserDeclines(UserThatDeclined);
            }
            else
            {
                return;
            }
        }
    }
}
