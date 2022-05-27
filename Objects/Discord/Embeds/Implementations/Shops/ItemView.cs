using Discord;
using Discord.WebSocket;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using Kaia.Bot.Objects.KaiaStructures.Users;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class ItemView : IKaiaItemContentView
    {
        public ItemView(CommandContext Context, KaiaInventoryItem Item, IEmote BuyItemEmote, IEmote InteractWithItemEmote)
        {
            this.Context = Context;
            this.Item = Item;
            this.BuyItemEmote = BuyItemEmote;
            this.InteractWithItemEmote = InteractWithItemEmote;
            this.Refreshed = false;
        }

        public CommandContext Context { get; }
        public KaiaInventoryItem Item { get; }
        public IEmote BuyItemEmote { get; }
        public IEmote InteractWithItemEmote { get; }
        public IEmote? GoBackView => Emotes.Embeds.Reverse;
        public ulong BId { get; } = IdGenerator.CreateNewId();
        public ulong IId { get; } = IdGenerator.CreateNewId();
        public ulong RId { get; } = IdGenerator.CreateNewId();
        public string BuyId => $"{this.Item.DisplayName}-{this.BId}";
        public string InteractId => $"{this.Item.DisplayName}-{this.IId}";
        public string BackId => $"{this.Item.DisplayName}-{this.RId}";

        private bool Refreshed { get; set; }

        public event IKaiaItemContentView.GoBackHandler? BackRequested;

        public Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = new ComponentBuilder()
                .WithButton("Buy",
                           this.BuyId,
                           ButtonStyle.Secondary,
                           this.BuyItemEmote,
                           disabled: U.Settings.Inventory.Petals < this.Item.Cost)
                .WithButton("Interact",
                           this.InteractId,
                           ButtonStyle.Secondary,
                           this.InteractWithItemEmote,
                           disabled: !U.Settings.Inventory.Items.Any(I => I.DisplayName == this.Item.DisplayName) || !this.Item.CanInteractWithDirectly);
            if(this.GoBackView != null)
            {
                CB.WithButton("Back", this.BackId, ButtonStyle.Secondary, this.GoBackView, disabled: false);
            }
            return Task.FromResult(CB);
        }

        public Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U)
        {
            KaiaPathEmbed Em = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop, this.Item.DisplayName);

            Em.WriteField($"[{Strings.Economy.CurrencyEmote} `{this.Item.Cost}`] {this.Item.DisplayName}  {this.Item.DisplayEmote}", this.Item.Description);
            Em.WriteField("your balance", $"{Strings.Economy.CurrencyEmote} `{U.Settings.Inventory.Petals}`{(this.Refreshed ? "- balances may go up due to passive income from books every time it refreshes." : "")}");
            Em.WriteField($"number of {this.Item.DisplayName}s owned", $"`{U.Settings.Inventory.Items.Count(I => I.DisplayName == this.Item.DisplayName)}`");
            this.Refreshed = true;
            return Task.FromResult(Em);
        }

        public async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbed E = await this.GetEmbedAsync(U);
            ComponentBuilder Com = await this.GetComponentsAsync(U);
            await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
            this.Context.Reference.Client.ButtonExecuted += this.ButtonExecutedAsync;
        }

        private async Task ButtonExecutedAsync(SocketMessageComponent Arg)
        {
            if ((Arg.Data.CustomId == this.BuyId || Arg.Data.CustomId == this.InteractId || Arg.Data.CustomId == this.BackId) && Arg.User.Id == this.Context.UserContext.User.Id)
            {
                if(Arg.Data.CustomId != this.BackId)
                {
                    KaiaUser U = new(Arg.User.Id);
                    if (Arg.Data.CustomId == this.BuyId && U.Settings.Inventory.Petals >= this.Item.Cost)
                    {
                        await this.Item.UserBoughtAsync(U);
                    }
                    else if (Arg.Data.CustomId == this.InteractId && U.Settings.Inventory.Items.Any(I => I.DisplayName == this.Item.DisplayName))
                    {
                        U.Settings.Inventory.Items.RemoveAt(U.Settings.Inventory.Items.FindIndex(C => C.DisplayName == this.Item.DisplayName));
                        await this.Item.UserInteractAsync(this.Context, U);
                    }
                    await U.SaveAsync();
                    KaiaPathEmbed E = await this.GetEmbedAsync(U);
                    ComponentBuilder Com = await this.GetComponentsAsync(U);
                    await Arg.UpdateAsync(C =>
                    {
                        C.Embed = E.Build();
                        C.Components = Com.Build();
                    });
                }
                else
                {
                    this.BackRequested?.Invoke(Arg);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.Client.ButtonExecuted -= this.ButtonExecutedAsync;
        }
    }
}
