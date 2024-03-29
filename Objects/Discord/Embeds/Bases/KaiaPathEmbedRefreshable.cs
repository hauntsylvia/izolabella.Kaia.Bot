﻿namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public abstract class KaiaPathEmbedRefreshable(string Parent, string? Sub1 = null, string? Sub2 = null, Color? Override = null) : KaiaPathEmbed(Parent, Sub1, Sub2, Override)
    {
        public async Task RefreshAsync()
        {
            if (!this.IsRefreshed)
            {
                this.IsRefreshed = true;
            }
            await this.ClientRefreshAsync();
        }

        public bool IsRefreshed { get; private set; }

        protected abstract Task ClientRefreshAsync();
    }
}