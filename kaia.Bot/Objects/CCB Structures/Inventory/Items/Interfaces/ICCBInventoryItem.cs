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
    public interface ICCBInventoryItem
    {
        string DisplayName { get; }
        Emoji DisplayEmote { get; }
        DateTime ReceivedAt { get; }
        Task UserBoughtAsync(CommandContext Context, CCBUser User);
        Task UserUsingAsync(CommandContext Context, CCBUser User);
    }
}
