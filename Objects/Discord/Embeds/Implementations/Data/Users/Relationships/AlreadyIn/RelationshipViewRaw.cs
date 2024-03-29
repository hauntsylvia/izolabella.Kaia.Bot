﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
{
    public class RelationshipViewRaw(UserRelationship Relationship) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Users, Strings.EmbedStrings.FakePaths.Relationships, Relationship.Id.ToString(CultureInfo.InvariantCulture))
    {
        public UserRelationship Relationship { get; } = Relationship;

        protected override Task ClientRefreshAsync()
        {
            List<string> Display = new()
        {
            "__members__"
        };
            Display.AddRange(this.Relationship.KaiaUserIds.Select(A => $"→ <@{A}>"));
            if (this.Relationship.PendingIds.Any())
            {
                Display.Add("__pending__");
                Display.AddRange(this.Relationship.PendingIds.Select(A => $"→ <@{A}>"));
            }
            this.WithListWrittenToField($"{this.Relationship.Emote} relationship {this.Relationship.Id}", Display, "\n");
            return Task.CompletedTask;
        }
    }
}