namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public abstract class KaiaPathEmbedRefreshable : KaiaPathEmbed
    {
        public KaiaPathEmbedRefreshable(string Parent, string? Sub1 = null, string? Sub2 = null, Color? Override = null) : base(Parent, Sub1, Sub2, Override)
        {
        }

        public void Refresh()
        {
            this.RefreshAsync().Wait();
        }

        public abstract Task RefreshAsync();
    }
}
