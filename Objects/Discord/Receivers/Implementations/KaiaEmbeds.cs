using Kaia.Bot.Objects.Discord.Embeds.Implementations.KaiaEmbeds.KaiaServer;

namespace Kaia.Bot.Objects.Discord.Receivers.Implementations
{
    public class KaiaEmbeds : Receiver
    {
        public override string Name => "Kaia Embeds";

        public override Task CallbackAsync(KaiaUser Author, SocketMessage Message, ReceiverResult CausedCallback)
        {
            return Task.CompletedTask;
        }

        public override Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message)
        {
            return Task.FromResult(Message.Author.Id == 528750326107602965 && Message.Content == "welcome!");
        }

        public override Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }

        public override async Task<ReceiverResult> OnMessageAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message)
        {
            await Message.Channel.SendMessageAsync(embed: new VerifyEmbed().Build());
            return new ReceiverResult();
        }
    }
}
