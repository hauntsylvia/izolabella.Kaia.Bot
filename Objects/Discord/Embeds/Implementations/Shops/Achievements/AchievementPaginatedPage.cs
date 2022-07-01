using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
{
    public class AchievementPaginatedPage : KaiaPathEmbedRefreshable
    {
        public AchievementPaginatedPage(IEnumerable<KaiaAchievement> Chunk, CommandContext Context, KaiaUser User) : base(Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Achievements)
        {
            this.Chunk = Chunk;
            this.Context = Context;
            this.User = User;
        }

        public IEnumerable<KaiaAchievement> Chunk { get; }

        public CommandContext Context { get; }

        public KaiaUser User { get; }

        protected override Task ClientRefreshAsync()
        {
            foreach(KaiaAchievement Ach in this.Chunk)
            {
                this.WithField($"{Ach.DisplayEmote} {Ach.Title} : `earned: {(Ach.UserAlreadyOwns(this.User).Result ? Emotes.Counting.Check : Emotes.Counting.Invalid)}`", $"{Ach.GetDescriptionAsync(this.User).Result}");
            }
            return Task.CompletedTask;
        }
    }
}
