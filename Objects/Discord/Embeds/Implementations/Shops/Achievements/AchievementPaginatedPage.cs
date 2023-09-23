using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
{
    public class AchievementPaginatedPage(IEnumerable<KaiaAchievement> Chunk, CommandContext Context, KaiaUser User) : KaiaPathEmbedRefreshable(Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Achievements)
    {
        public IEnumerable<KaiaAchievement> Chunk { get; } = Chunk;

        public CommandContext Context { get; } = Context;

        public KaiaUser User { get; } = User;

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