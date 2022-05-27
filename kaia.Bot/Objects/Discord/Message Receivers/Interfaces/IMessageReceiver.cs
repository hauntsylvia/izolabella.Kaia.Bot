using Discord.WebSocket;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Derivations;
using Kaia.Bot.Objects.Discord.Message_Receivers.Results;

namespace Kaia.Bot.Objects.Discord.Events.Interfaces
{
    public interface IMessageReceiver : ISelfHandler
    {
        Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message);
        Task<MessageReceiverResult> RunAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message);
        Task CallbackAsync(KaiaUser Author, SocketMessage Message, MessageReceiverResult CausedCallback);
        string Name { get; }
    }
}
