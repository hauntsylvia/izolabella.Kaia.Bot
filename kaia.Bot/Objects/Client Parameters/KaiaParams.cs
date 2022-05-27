using Discord;
using Discord.WebSocket;
using Kaia.Bot.Objects.CCB_Structures;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Clients;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Commands.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(T != null)
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
