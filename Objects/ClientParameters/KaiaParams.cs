namespace izolabella.Kaia.Bot.Objects.ClientParameters
{
    public class KaiaParams(DiscordSocketConfig Config, bool AllowBotsOnMessageReceivers, bool GlobalCommands, string Token)
    {
        public IzolabellaDiscordClient CommandHandler { get; } = new(Config, GlobalCommands);

        public bool AllowBotsOnMessageReceivers { get; } = AllowBotsOnMessageReceivers;

        private string Token { get; set; } = Token;

        public async Task StartAsync()
        {
            await this.CommandHandler.StartAsync(this.Token, false);
        }

        public async Task StopAsync()
        {
            await this.CommandHandler.StopAndLogoutAsync();
        }
    }
}