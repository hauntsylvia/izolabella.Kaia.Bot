using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
{
    public class AchievementRawView(CommandContext Context, KaiaAchievement KaiaAchievement, KaiaUser User) : KaiaPathEmbedRefreshable(Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Achievements, KaiaAchievement.Title)
    {
        public CommandContext Context { get; } = Context;

        public KaiaAchievement KaiaAchievement { get; } = KaiaAchievement;

        public KaiaUser User { get; } = User;

        protected override async Task ClientRefreshAsync()
        {
            this.WithField("description", $"`{await this.KaiaAchievement.GetDescriptionAsync(this.User)}`");
            this.WithField("achieved?", $"`{(await this.KaiaAchievement.UserAlreadyOwns(this.User) ? "yes" : "no")}`");
            double TotalCurrencyEarned = this.KaiaAchievement.Rewards.Sum(A => A.Petals);
            List<string> W = new()
        {
            $"{Strings.Economy.CurrencyEmote} `{TotalCurrencyEarned}`",
            Strings.EmbedStrings.Empty,
        };
            foreach (KaiaUserReward Reward in this.KaiaAchievement.Rewards)
            {
                foreach (KaiaInventoryItem Item in Reward.Items)
                {
                    W.Add($"{Item.DisplayEmote} {Item.DisplayName}");
                }
            }
            this.WithListWrittenToField("rewards", W, "\n");
        }
    }
}