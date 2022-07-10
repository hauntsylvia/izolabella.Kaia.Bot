using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.Pending
{
    internal class PendingRelInviteDisplayRaw : KaiaPathEmbedRefreshable
    {
        public PendingRelInviteDisplayRaw(UserRelationship Rel) : base(Strings.EmbedStrings.FakePaths.Users, Strings.EmbedStrings.FakePaths.Relationships, Rel.Id.ToString(CultureInfo.InvariantCulture))
        {
            this.Rel = Rel;
        }

        public UserRelationship Rel { get; }

        protected override Task ClientRefreshAsync()
        {
            WithListWrittenToField("members", Rel.KaiaUserIds.Select(S => $"<@{S}>"), ",\n");
            WithField("invited", $"{Rel.PendingIds.Count()} users");
            return Task.CompletedTask;
        }
    }
}
