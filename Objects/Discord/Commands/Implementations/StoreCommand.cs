using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class StoreCommand : IKaiaCommand
    {
        public string Name => "Store";

        public string Description => "Display the global store, or buy an item.";

        public bool GuildsOnly => false;

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Quantity", "The number of items to buy.", ApplicationCommandOptionType.Integer, false)
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.StoreOrBuy;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? ItemArg = Arguments.FirstOrDefault(A =>
            {
                return A.Name.ToLower(CultureInfo.InvariantCulture) == "item";
            });
            IzolabellaCommandArgument? QuantityArg = Arguments.FirstOrDefault(A =>
            {
                return A.Name.ToLower(CultureInfo.InvariantCulture) == "quantity";
            });
            if (ItemArg != null && QuantityArg != null && ItemArg.Value is string ItemName && QuantityArg.Value is long Quantity)
            {
                KaiaInventoryItem? InventoryItem = InterfaceImplementationController.GetItems<KaiaInventoryItem>().FirstOrDefault(III =>
                {
                    return III.DisplayName == ItemName;
                });
                if (InventoryItem != null && Quantity > 0)
                {
                    KaiaUser User = new(Context.UserContext.User.Id);
                    double TotalCost = InventoryItem.Cost * Quantity;
                    if (TotalCost <= User.Settings.Inventory.Petals)
                    {
                        List<KaiaInventoryItem> ItemsBought = new();
                        for (long Q = 0; Q < Quantity; Q++)
                        {
                            object? O = Activator.CreateInstance(InventoryItem.GetType());
                            if (O is not null and KaiaInventoryItem NewItem)
                            {
                                await NewItem.UserBoughtAsync(User);
                                ItemsBought.Add(NewItem);
                            }
                        }
                        Dictionary<KaiaPathEmbed, List<SelectMenuOptionBuilder>?> Embeds = new();
                        List<KaiaInventoryItem[]> ItemsBoughtChunked = ItemsBought.Chunk(10).ToList();
                        foreach (KaiaInventoryItem[] ItemArray in ItemsBoughtChunked)
                        {
                            Embeds.Add(new StoreTransactionCompleted(User, ItemArray.ToList()), null);
                        }
                        await User.SaveAsync();
                        KaiaPathEmbedPaginated P = new(Embeds,
                                                       new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop),
                                                       Context,
                                                       0,
                                                       Emotes.Embeds.Back,
                                                       Emotes.Embeds.Forward,
                                                       Strings.EmbedStrings.FakePaths.Global,
                                                       Strings.EmbedStrings.FakePaths.StoreOrShop);
                        await P.StartAsync();
                    }
                    else
                    {
                        await Context.UserContext.RespondAsync(text: Strings.Responses.Commands.InvalidCurrencyAmount);
                    }
                }
                else
                {
                    await Context.UserContext.RespondAsync(text: Quantity > 0 ? Strings.Responses.Commands.NoInventoryItemWithThatNameFound : Strings.Responses.Commands.ZeroOrNegativeQuantity);
                }
            }
            else
            {
                List<KaiaInventoryItem> Items = InterfaceImplementationController.GetItems<KaiaInventoryItem>();
                ItemsPage E = new(Context, Items, 6);
                await E.StartAsync();
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            List<IzolabellaCommandParameterChoices> Choices = new();
            foreach (KaiaInventoryItem Item in InterfaceImplementationController.GetItems<KaiaInventoryItem>())
            {
                Choices.Add(new($"[{Item.DisplayEmote}] {Item.DisplayName}", Item.DisplayName));
            }
            this.Parameters.Add(new("Item", "The item to buy.", ApplicationCommandOptionType.String, false)
            {
                Choices = Choices
            });
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
