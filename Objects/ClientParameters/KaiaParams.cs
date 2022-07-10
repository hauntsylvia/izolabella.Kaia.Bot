namespace izolabella.Kaia.Bot.Objects.ClientParameters
{
    public class KaiaParams
    {
        public KaiaParams(DiscordSocketConfig Config, bool AllowBotsOnMessageReceivers, bool GlobalCommands, string Token)
        {
            CommandHandler = new(Config, GlobalCommands);
            this.AllowBotsOnMessageReceivers = AllowBotsOnMessageReceivers;
            this.Token = Token;
        }

        public IzolabellaDiscordClient CommandHandler { get; }

        public bool AllowBotsOnMessageReceivers { get; }

        private string Token { get; set; }

        public async Task StartAsync()
        {
            await CommandHandler.StartAsync(Token, false);
        }

        public async Task StopAsync()
        {
            await CommandHandler.StopAndLogoutAsync();
        }
    }
}
