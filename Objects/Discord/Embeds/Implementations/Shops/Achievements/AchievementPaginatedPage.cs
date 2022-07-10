using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
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
            foreach (KaiaAchievement Ach in this.Chunk)
            {
                this.WithField($"{Ach.DisplayEmote} {Ach.Title} : `earned: {(Ach.UserAlreadyOwns(this.User).Result ? Emotes.Counting.Check : Emotes.Counting.Invalid)}`", $"{Ach.GetDescriptionAsync(this.User).Result}");
            }
            return Task.CompletedTask;
        }
    }
}
