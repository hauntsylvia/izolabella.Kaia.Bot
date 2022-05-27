using Discord;
using Kaia.Bot.Objects.CCB_Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Bases
{
    public interface ICCBItemContentView : IDisposable
    {
        public IEmote BuyItemEmote { get; }
        public Task<CCBPathEmbed> GetEmbedAsync(KaiaUser U);
        public Task<ComponentBuilder> GetComponentsAsync(KaiaUser U);
        public Task StartAsync(KaiaUser U);
    }
}
