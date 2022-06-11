using Kaia.Bot.Objects.ErrorControl.Interfaces;

namespace Kaia.Bot.Objects.Discord.MessageReceivers.Interfaces
{
    public interface IMessageReceiver : ISelfHandler
    {
        Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message);
        Task<MessageReceiverResult> RunAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message);
        Task CallbackAsync(KaiaUser Author, SocketMessage Message, MessageReceiverResult CausedCallback);
        string Name { get; }
    }
}
