﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
{
    public class CreateNewRelationshipViewRaw(CommandContext Context, UserRelationship Rel) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Users,
        Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Relationships)
    {
        public CommandContext Context { get; } = Context;

        public UserRelationship Rel { get; } = Rel;

        protected override Task ClientRefreshAsync()
        {
            this.WithListWrittenToField("relationship ", this.Rel.KaiaUserIds.Select(S => $"<@{S}>"), ",\n");
            this.WithListWrittenToField("pending", this.Rel.PendingIds.Select(S => $"<@{S}>"), ",\n");
            return Task.CompletedTask;
        }
    }
}