using Discord;
using Discord.WebSocket;
using izolabella.Discord.Objects.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Client_Parameters
{
    public class ClientParameters
    {
        public ClientParameters(DiscordSocketConfig Config, string? Token)
        {
            this.Wrapper = new(Config);
            this.Token = Token;
        }

        public IzolabellaDiscordCommandClient Wrapper { get; }
        private string? Token { get; }

        public async Task StartAsync(string? Token = null)
        {
            string? T = this.Token ?? Token;
            if(T != null)
            {
                await this.Wrapper.StartAsync(T);
            }
            else
            {
                throw new NullReferenceException(nameof(this.Token));
            }
        }
    }
}
