using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
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
