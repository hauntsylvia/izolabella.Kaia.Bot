using Discord;
using izolabella.Discord.Objects.Arguments;
using izolabella.Storage.Objects.Structures;
using Kaia.Bot.Objects.CCB_Structures.Derivations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases
{
    public interface IKaiaInventoryItem
    {
        string DisplayName { get; }
        string Description { get; }
        double Cost { get; }
        bool CanInteractWithDirectly { get; }
        Emoji DisplayEmote { get; }
        DateTime ReceivedAt { get; }
        Task UserBoughtAsync(CommandContext Context, KaiaUser User);
        Task UserInteractAsync(CommandContext Context, KaiaUser User);
    }
}
