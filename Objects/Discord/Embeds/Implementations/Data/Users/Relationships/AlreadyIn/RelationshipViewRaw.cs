using Kaia.Bot.Objects.KaiaStructures.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
{
    public class RelationshipViewRaw : KaiaPathEmbedRefreshable
    {
        public RelationshipViewRaw(UserRelationship Relationship) : base(Strings.EmbedStrings.FakePaths.Users, Strings.EmbedStrings.FakePaths.Relationships, Relationship.Id.ToString(CultureInfo.InvariantCulture))
        {
            this.Relationship = Relationship;
        }

        public UserRelationship Relationship { get; }

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
