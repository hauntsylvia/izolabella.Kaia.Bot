using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Util;
using System.Collections.Generic;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class StoreCommand : IKaiaCommand
    {
        public string Name => "Store";

        public string Description => "Display the global store, or buy an item.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {            
            new("User Listings", "Whether to include user listings or not.", ApplicationCommandOptionType.Boolean, false),
            new("Lister", "Filters all listings by this user.", ApplicationCommandOptionType.User, false)
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.StoreOrBuy;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? IncludeUserListings = Arguments.FirstOrDefault(A => A.Name == "user-listings");
            IzolabellaCommandArgument? Lister = Arguments.FirstOrDefault(A => A.Name == "lister");
            IUser? User = null;
            if(Lister != null && Lister.Value is IUser U)
            {
                User = U;
            }
            ItemsPaginated E = new(Context, User, IncludeUserListings?.Value is not bool B || B);
            await E.StartAsync();
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
