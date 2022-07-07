using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Self;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using Kaia.Bot.Objects.KaiaStructures.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class MyRelationshipsPage : KaiaPathEmbedRefreshable
    {
        public MyRelationshipsPage(CommandContext Context, IEnumerable<UserRelationship> Relationships) : base(Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Relationships)
        {
            this.Context = Context;
            this.Relationships = Relationships;
        }

        public CommandContext Context { get; }

        public IEnumerable<UserRelationship> Relationships { get; }

        protected override Task ClientRefreshAsync()
        {
            foreach(UserRelationship Relationship in this.Relationships)
            {
                IEnumerable<string> Display = Relationship.KaiaUserIds.Select(A => $"<@{A}>");
                this.WithListWrittenToField($"members", Display, ",\n");
            }
            return Task.CompletedTask;
        }
    }
}
