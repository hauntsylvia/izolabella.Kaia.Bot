﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
{
    public class MyRelationshipsPage(CommandContext Context, int MaxMembersToDisplay, IEnumerable<UserRelationship> Relationships) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Relationships)
    {
        public CommandContext Context { get; } = Context;

        public int MaxMembersToDisplay { get; } = MaxMembersToDisplay;

        public IEnumerable<UserRelationship> Relationships { get; } = Relationships;

        protected override Task ClientRefreshAsync()
        {
            foreach (UserRelationship Relationship in this.Relationships)
            {
                List<string> Display = new()
            {
                "__members__"
            };
                Display.AddRange(Relationship.KaiaUserIds.Take(this.MaxMembersToDisplay).Select(A => $"→ <@{A}>"));
                if (Relationship.KaiaUserIds.Count() > this.MaxMembersToDisplay)
                {
                    Display.Add($". . and {Relationship.KaiaUserIds.Count() - this.MaxMembersToDisplay} more");
                }

                if (Relationship.PendingIds.Any())
                {
                    Display.Add("__pending__");
                    Display.AddRange(Relationship.PendingIds.Take(this.MaxMembersToDisplay).Select(A => $"→ <@{A}>"));

                    if (Relationship.PendingIds.Count() > this.MaxMembersToDisplay)
                    {
                        Display.Add($". . and {Relationship.PendingIds.Count() - this.MaxMembersToDisplay} more");
                    }
                }
                this.WithListWrittenToField($"{Relationship.Emote} relationship {Relationship.Id}", Display, "\n");
            }
            return Task.CompletedTask;
        }
    }
}