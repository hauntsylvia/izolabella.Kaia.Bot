using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
{
    public class AchievementRawView : KaiaPathEmbedRefreshable
    {
        public AchievementRawView(CommandContext Context, KaiaAchievement KaiaAchievement, KaiaUser User) : base(Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Achievements, KaiaAchievement.Title)
        {
            this.Context = Context;
            this.KaiaAchievement = KaiaAchievement;
            this.User = User;
        }

        public CommandContext Context { get; }

        public KaiaAchievement KaiaAchievement { get; }

        public KaiaUser User { get; }

        protected override async Task ClientRefreshAsync()
        {
            WithField("description", $"`{await KaiaAchievement.GetDescriptionAsync(User)}`");
            WithField("achieved?", $"`{(await KaiaAchievement.UserAlreadyOwns(User) ? "yes" : "no")}`");
            double TotalCurrencyEarned = KaiaAchievement.Rewards.Sum(A => A.Petals);
            List<string> W = new()
            {
                $"{Strings.Economy.CurrencyEmote} `{TotalCurrencyEarned}`",
                Strings.EmbedStrings.Empty,
            };
            foreach (KaiaUserReward Reward in KaiaAchievement.Rewards)
            {
                foreach (KaiaInventoryItem Item in Reward.Items)
                {
                    W.Add($"{Item.DisplayEmote} {Item.DisplayName}");
                }
            }
            WithListWrittenToField("rewards", W, "\n");
        }
    }
}
