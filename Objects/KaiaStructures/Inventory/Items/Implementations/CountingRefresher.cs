using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Interfaces;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class CountingRefresher : IKaiaInventoryItem
    {
        public string DisplayName => "Counting Refresher";

        public string Description => "allows u to keep counting even if u fail.";

        public double Cost => 50;

        public Emoji DisplayEmote => Emotes.Items.CountingRefresher;

        public DateTime ReceivedAt { get; private set; } = DateTime.UtcNow;

        public bool CanInteractWithDirectly => false;

        public Task UserBoughtAsync(CommandContext Context, KaiaUser User)
        {
            this.ReceivedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public Task UserInteractAsync(CommandContext Context, KaiaUser User)
        {
            return Task.CompletedTask;
        }
    }
}
