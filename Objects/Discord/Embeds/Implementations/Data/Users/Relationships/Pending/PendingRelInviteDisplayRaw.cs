using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.Pending
{
    internal sealed class PendingRelInviteDisplayRaw(UserRelationship Rel) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Users, Strings.EmbedStrings.FakePaths.Relationships, Rel.Id.ToString(CultureInfo.InvariantCulture))
    {
        public UserRelationship Rel { get; } = Rel;

        protected override Task ClientRefreshAsync()
        {
            this.WithListWrittenToField("members", this.Rel.KaiaUserIds.Select(S => $"<@{S}>"), ",\n");
            this.WithField("invited", $"{this.Rel.PendingIds.Count()} users");
            return Task.CompletedTask;
        }
    }
}