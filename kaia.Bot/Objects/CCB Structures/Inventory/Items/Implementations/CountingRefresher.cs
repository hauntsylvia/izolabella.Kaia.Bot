using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using Kaia.Bot.Objects.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Implementations
{
    public class CountingRefresher : ICCBInventoryItem
    {
        public string DisplayName => "Counting Refresher";

        public string Description => "allows u to keep counting even if u fail.";

        public double Cost => 50;

        public Emoji DisplayEmote => Emotes.Items.CountingRefresher;

        public DateTime ReceivedAt { get; private set; } = DateTime.UtcNow;

        public bool CanInteractWithDirectly => false;

        public Task UserBoughtAsync(CommandContext Context, CCBUser User)
        {
            this.ReceivedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public Task UserInteractAsync(CommandContext Context, CCBUser User)
        {
            return Task.CompletedTask;
        }
    }
}
