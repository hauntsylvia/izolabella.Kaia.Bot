﻿using Discord;
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
    internal class CountingRefresher : ICCBInventoryItem
    {
        public string DisplayName => "Counting Refresher";

        public Emoji DisplayEmote => Emotes.Items.CountingRefresher;

        public DateTime ReceivedAt { get; private set; } = DateTime.UtcNow;

        public Task UserBoughtAsync(CommandContext Context, CCBUser User)
        {
            this.ReceivedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public Task UserUsingAsync(CommandContext Context, CCBUser User)
        {
            throw new NotImplementedException();
        }
    }
}
