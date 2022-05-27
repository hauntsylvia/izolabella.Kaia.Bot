using Discord;
using Discord.WebSocket;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public interface IKaiaItemContentView : IDisposable
    {
        public IEmote? GoBackView { get; }

        public delegate void GoBackHandler(SocketMessageComponent Component);

        public event GoBackHandler? BackRequested;
        public Task StartAsync(KaiaUser U);
        public Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U);
        public Task<ComponentBuilder> GetComponentsAsync(KaiaUser U);
    }
}
