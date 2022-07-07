using Kaia.Bot.Objects.KaiaStructures.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships
{
    public class CreateNewRelationshipViewRaw : KaiaPathEmbedRefreshable
    {
        public CreateNewRelationshipViewRaw(CommandContext Context, UserRelationship Rel) : base(Strings.EmbedStrings.FakePaths.Users,
            Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Relationships)
        {
            this.Context = Context;
            this.Rel = Rel;
        }

        public CommandContext Context { get; }

        public UserRelationship Rel { get; }

        protected override Task ClientRefreshAsync()
        {
            this.WithListWrittenToField("members", this.Rel.KaiaUserIds.Select(S => $"<@{S}>"), ",\n");
            return Task.CompletedTask;
        }
    }
}
