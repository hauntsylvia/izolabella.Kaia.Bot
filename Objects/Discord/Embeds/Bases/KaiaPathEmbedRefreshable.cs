namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public abstract class KaiaPathEmbedRefreshable : KaiaPathEmbed
    {
        public KaiaPathEmbedRefreshable(string Parent, string? Sub1 = null, string? Sub2 = null, Color? Override = null) : base(Parent, Sub1, Sub2, Override)
        {
        }

        public async Task RefreshAsync()
        {
            if (!IsRefreshed)
            {
                IsRefreshed = true;
            }
            await ClientRefreshAsync();
        }

        public bool IsRefreshed { get; private set; }

        protected abstract Task ClientRefreshAsync();
    }
}
