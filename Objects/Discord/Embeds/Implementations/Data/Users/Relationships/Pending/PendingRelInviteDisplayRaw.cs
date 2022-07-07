using Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.Pending
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
            this.WithListWrittenToField("members", this.Rel.KaiaUserIds.Select(S => $"<@{S}>"), ",\n");
            this.WithListWrittenToField("pending", this.Rel.PendingIds.Select(S => $"<@{S}>"), ",\n");
            return Task.CompletedTask;
        }
    }
}
