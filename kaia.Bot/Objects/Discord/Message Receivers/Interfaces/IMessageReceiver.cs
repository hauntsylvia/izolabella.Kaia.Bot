using Discord.WebSocket;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures.Derivations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Events.Interfaces
{
    public interface IMessageReceiver : ISelfHandler
    {
        Task<bool> CheckMessageValidityAsync(CCBUser Author, SocketMessage Message);
        Task RunAsync(CCBUser Author, SocketMessage Message);
        string Name { get; }
    }
}
