using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;

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

        public override Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            KaiaPathEmbedRefreshable Em = new AchievementRawView(this.Context, this.Achievement, U);
            return Task.FromResult(Em);
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
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
