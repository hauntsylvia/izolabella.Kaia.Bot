using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
{
    public class AchievementView : KaiaItemContentView
    {
        public AchievementView(AchievementsPaginated P, KaiaAchievement Achievement, CommandContext Context) : base(P, Context, true)
        {
            this.Achievement = Achievement;
        }

        public KaiaAchievement Achievement { get; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override async Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U)
        {
            KaiaPathEmbed Em = new(this.Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Achievements, this.Achievement.Title);
            Em.WithField("description", $"`{await this.Achievement.GetDescriptionAsync(U)}`");
            Em.WithField("achieved?", $"`{(await this.Achievement.UserAlreadyOwns(U) ? "yes" : "no")}`");
            double TotalCurrencyEarned = this.Achievement.Rewards.Sum(A => A.Petals);
            List<string> W = new()
            {
                $"{Strings.Economy.CurrencyEmote} `{TotalCurrencyEarned}`",
                Strings.EmbedStrings.Empty,
            };
            foreach (KaiaAchievementReward Reward in this.Achievement.Rewards)
            {
                foreach (KaiaInventoryItem Item in Reward.Items)
                {
                    W.Add($"{Item.DisplayEmote} {Item.DisplayName}");
                }
            }
            Em.WithListWrittenToField("rewards", W, "\n");
            return Em;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbed E = await this.GetEmbedAsync(U);
            ComponentBuilder C = await this.GetDefaultComponents();
            _ = await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = C.Build();
                M.Embed = E.Build();
            });
        }
    }
}
