using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Discord.WebSocket;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using Discord;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class StoreCommand : ICCBCommand
    {
        public string Name => "Store";

        public string Description => "Display the global store, or buy an item.";

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            new("Quantity", "The number of items to buy.", ApplicationCommandOptionType.Integer, false)
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.StoreOrBuy;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? ItemArg = Arguments.FirstOrDefault(A => A.Name.ToLower() == "item");
            IzolabellaCommandArgument? QuantityArg = Arguments.FirstOrDefault(A => A.Name.ToLower() == "quantity");
            if (ItemArg != null && QuantityArg != null && ItemArg.Value is string ItemName && QuantityArg.Value is long Quantity)
            {
                ICCBInventoryItem? InventoryItem = InterfaceImplementationController.GetItems<ICCBInventoryItem>().FirstOrDefault(III => III.DisplayName == ItemName);
                if(InventoryItem != null && Quantity > 0)
                {
                    CCBUser User = new(Context.UserContext.User.Id);
                    double TotalCost = InventoryItem.Cost * Quantity;
                    if (TotalCost <= User.Settings.Inventory.Petals)
                    {
                        List<ICCBInventoryItem> ItemsBought = new();
                        for (long Q = 0; Q < Quantity; Q++)
                        {
                            object? O = Activator.CreateInstance(InventoryItem.GetType());
                            if (O != null && O is ICCBInventoryItem NewItem)
                            {
                                await NewItem.UserBoughtAsync(Context, User);
                                User.Settings.Inventory.Items.Add(NewItem);
                                ItemsBought.Add(NewItem);
                                User.Settings.Inventory.Petals -= NewItem.Cost;
                            }
                        }
                        List<CCBPathEmbed> Embeds = new();
                        List<ICCBInventoryItem[]> ItemsBoughtChunked = ItemsBought.Chunk(10).ToList();
                        foreach (ICCBInventoryItem[] ItemArray in ItemsBoughtChunked)
                        {
                            Embeds.Add(new StoreTransactionCompleted(User, ItemArray.ToList()));
                        }
                        await User.SaveAsync();
                        CCBPathPaginatedEmbed P = new(Embeds, new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop), Context, 0, Emotes.Embeds.Back, Emotes.Embeds.Forward, Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop);
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
                CCBPathPaginatedEmbed E = new(new List<CCBPathEmbed>()
                {
                    new StorePage("1", new(Context.UserContext.User.Id), InterfaceImplementationController.GetItems<ICCBInventoryItem>()),
                }, new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop), Context, 0, Emotes.Embeds.Back, Emotes.Embeds.Forward, "a");
                await E.StartAsync();
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            List<IzolabellaCommandParameterChoices> Choices = new();
            foreach (ICCBInventoryItem Item in InterfaceImplementationController.GetItems<ICCBInventoryItem>())
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
