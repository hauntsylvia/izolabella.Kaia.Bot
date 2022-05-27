using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class CountingRefresher : KaiaInventoryItem
    {
        public CountingRefresher() : base(DisplayName: "Counting Refresher",
                                          Description: "allows u to keep counting even if u fail.",
                                          Cost: 150,
                                          CanInteractWithDirectly: false,
                                          DisplayEmoteName: Emotes.Items.CountingRefresher)
        {

        }

        public override Task UserInteractAsync(CommandContext Context, KaiaUser User)
        {
            return Task.CompletedTask;
        }
    }
}
