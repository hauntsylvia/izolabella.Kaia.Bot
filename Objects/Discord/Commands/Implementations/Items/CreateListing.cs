using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Util;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using System.Linq;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Items
{
    public class CreateListing : IKaiaCommand
    {
        public string ForeverId => CommandForeverIds.CreateItemListing;

        public string Name => "Store Listing";

        public string Description => "Create a store listing.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Quantity", "Number of the item to put up for this listing.", ApplicationCommandOptionType.Number, true),
            new("Cost Per Item", $"Number of {Strings.Economy.CurrencyName} required to buy one item.", ApplicationCommandOptionType.Number, true),
        };

        public List<IIzolabellaCommandConstraint> Constraints => new();

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            IzolabellaCommandParameter Items = new("Item", "The item to put up in this listing.", ApplicationCommandOptionType.String, true)
            {
                Choices = BaseImplementationUtil.GetItems<KaiaInventoryItem>()
                                                .Where(Item => Item.UsersCanSellThis)
                                                .Select<KaiaInventoryItem, IzolabellaCommandParameterChoices>(Item => new($"[{Item.DisplayEmote}] {Item.DisplayName}", Item.DisplayName)).ToList()
            };
            this.Parameters.Add(Items);
            return Task.CompletedTask;
        }

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? QuantityArg = Arguments.FirstOrDefault(A => A.Name == "quantity");
            IzolabellaCommandArgument? CPIArg = Arguments.FirstOrDefault(A => A.Name == "cost-per-item");
            IzolabellaCommandArgument? ItemArg = Arguments.FirstOrDefault(A => A.Name == "item");
            if (QuantityArg != null && CPIArg != null && ItemArg != null && QuantityArg.Value is double Quantity && CPIArg.Value is double CPI && ItemArg.Value is string DisplayName)
            {
                if(!(await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>()).Any(S => S.ListerId == Context.UserContext.User.Id && S.IsListed))
                {
                    KaiaUser U = new(Context.UserContext.User.Id);
                    List<KaiaInventoryItem> ItemsFromUserInv = (await U.Settings.Inventory.GetItemsOfDisplayName(DisplayName)).Take((int)Quantity).ToList();
                    SaleListing SListing = new(ItemsFromUserInv, U, CPI);
                    await SListing.StartSellingAsync();
                    await new ItemView(new ItemsPaginated(Context, null, true), Context, SListing, Emotes.Counting.BuyItem, Emotes.Counting.InteractItem, Emotes.Counting.SellItem, true).StartAsync(U);
                }
                else
                {
                    await Context.UserContext.RespondAsync(text: "Users can only have a single listing.");
                }
            }
        }
    }
}
