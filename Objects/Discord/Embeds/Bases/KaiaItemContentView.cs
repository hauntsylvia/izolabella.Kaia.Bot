using Discord;
using Kaia.Bot.Objects.KaiaStructures;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public interface IKaiaItemContentView : IDisposable
    {
        public IEmote BuyItemEmote { get; }
        public Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U);
        public Task<ComponentBuilder> GetComponentsAsync(KaiaUser U);
        public Task StartAsync(KaiaUser U);
    }
}
