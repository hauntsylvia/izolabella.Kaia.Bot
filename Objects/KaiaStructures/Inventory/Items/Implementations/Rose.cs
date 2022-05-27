using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations
{
    public class Rose : KaiaInventoryItem
    {
        public Rose() : base(Strings.ItemStrings.Rose.Name, "a rose!", 72.34, true, Emotes.Items.Rose)
        {

        }

        public override async Task UserInteractAsync(CommandContext Context, KaiaUser User)
        {
            double PetalsEarned = Math.Round(new Random().Next(-100, 100) + new Random().NextDouble(), 2);
            string Message = (PetalsEarned < 0 ? Strings.ItemStrings.Rose.RoseStab : Strings.ItemStrings.Rose.RosePretty) + $" {Strings.Economy.CurrencyEmote} {(PetalsEarned < 0 ? "-" : "+")}`{Math.Abs(PetalsEarned)}`";
            if (Context.UserContext.HasResponded)
            {
                await Context.UserContext.FollowupAsync(Message);
            }
            else
            {
                await Context.UserContext.RespondAsync(Message);
            }
            User.Settings.Inventory.Petals += PetalsEarned;
        }
    }
}
