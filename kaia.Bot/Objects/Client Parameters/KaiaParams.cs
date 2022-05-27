using Discord.WebSocket;
using izolabella.Discord.Objects.Clients;

namespace Kaia.Bot.Objects.Client_Parameters
{
    public class KaiaParams
    {
        public KaiaParams(DiscordSocketConfig Config, bool AllowBotsOnMessageReceivers, bool GlobalCommands, string? Token)
        {
            this.CommandHandler = new(Config, GlobalCommands);
            this.AllowBotsOnMessageReceivers = AllowBotsOnMessageReceivers;
            this.Token = Token;
        }

        public IzolabellaDiscordCommandClient CommandHandler { get; }
        public bool AllowBotsOnMessageReceivers { get; }
        private string? Token { get; }
        public async Task StartAsync(string? Token = null)
        {
            string? T = this.Token ?? Token;
            if (T != null)
            {
                await this.CommandHandler.StartAsync(T, false);
            }
            else
            {
                throw new NullReferenceException(nameof(this.Token));
            }
        }
    }
}
