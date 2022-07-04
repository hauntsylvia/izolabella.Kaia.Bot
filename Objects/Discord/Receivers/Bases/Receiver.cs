using Kaia.Bot.Objects.Clients;
using Kaia.Bot.Objects.ErrorControl.Interfaces;

namespace Kaia.Bot.Objects.Discord.Receivers.Bases
{
    public abstract class Receiver : ISelfHandler
    {
        public abstract string Name { get; }

        public abstract Task CallbackAsync(KaiaUser Author, SocketMessage Message, ReceiverResult CausedCallback);

        public abstract Task OnErrorAsync(Exception Exception);

        public virtual Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message)
        {
            return Task.FromResult(false);
        }

        public virtual Task<ReceiverResult> OnReactionAsync(KaiaBot From, SocketReaction Reaction, bool Removing = false)
        {
            return Task.FromResult(new ReceiverResult());
        }

        public virtual Task<ReceiverResult> OnMessageAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message)
        {
            return Task.FromResult(new ReceiverResult());
        }
    }
}
