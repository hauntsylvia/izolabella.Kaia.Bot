using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements;

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

    public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
    {
        KaiaPathEmbedRefreshable Embed = new AchievementRawView(this.Context, this.Achievement, U);
        await Embed.RefreshAsync();
        return Embed;
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
