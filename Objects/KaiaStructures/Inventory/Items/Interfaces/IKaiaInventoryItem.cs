﻿using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Interfaces
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
