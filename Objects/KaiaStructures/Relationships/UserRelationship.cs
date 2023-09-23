using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Storage.Objects.Structures;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships
{
    public class UserRelationship(DateTime CreatedAt, string Description, KaiaEmote Emote, Dictionary<ulong, DateTime> PendingIds, Dictionary<ulong, DateTime> KaiaUserIds, ulong? Id = null) : IDataStoreEntity
    {
        public DateTime CreatedAt { get; } = CreatedAt;

        [JsonProperty(nameof(Description))]
        private readonly string description = Description.Length > 512 ? Description[..512] : Description;

        [JsonIgnore]
        public string Description => this.description.Length > 32 ? this.description[..32] : this.description.Length == 0 ? Strings.EmbedStrings.Empty : this.description;

        public KaiaEmote Emote { get; } = Emote;

        public ulong Id { get; } = Id ?? IdGenerator.CreateNewId();

        [JsonProperty(nameof(KaiaUserIds))]
        private readonly Dictionary<ulong, DateTime> kaiaUserIds = KaiaUserIds;

        [JsonIgnore]
        public IEnumerable<ulong> KaiaUserIds => this.kaiaUserIds.Keys.Distinct().ToArray();

        [JsonProperty("PendingUserIds")]
        private readonly Dictionary<ulong, DateTime> pendingIds = PendingIds;

        [JsonIgnore]
        public IEnumerable<ulong> PendingIds => this.pendingIds.Where(A => DateTime.UtcNow.Subtract(A.Value).TotalDays < 1).Select(KV => KV.Key).Distinct();

        [JsonIgnore]
        public int NumberOfMembers => this.KaiaUserIds.Count();

        [JsonIgnore]
        public bool AtMax => this.KaiaUserIds.Count() >= 50;

        public bool AddPendingMember(ulong Member)
        {
            if (this.AtMax)
            {
                return false;
            }
            this.pendingIds.TryAdd(Member, DateTime.UtcNow);
            return true;
        }

        public bool AddMember(ulong Member)
        {
            if (this.AtMax)
            {
                return false;
            }
            this.UserDeclines(Member);
            if (!this.kaiaUserIds.ContainsKey(Member))
            {
                this.kaiaUserIds.Add(Member, DateTime.UtcNow);
            }
            return true;
        }

        public void RemoveMember(ulong Member)
        {
            this.UserDeclines(Member);
            if (this.kaiaUserIds.ContainsKey(Member))
            {
                this.kaiaUserIds.Remove(Member);
                this.RemoveMember(Member);
            }
            else
            {
                return;
            }
        }

        public void UserDeclines(ulong UserThatDeclined)
        {
            if (this.pendingIds.ContainsKey(UserThatDeclined))
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