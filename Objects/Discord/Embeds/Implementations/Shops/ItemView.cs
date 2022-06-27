using izolabella.Storage.Objects.DataStores;
using izolabella.Util;
using izolabella.Util.RateLimits.Limiters;
using Kaia.Bot.Objects.Constants.Embeds;
using Kaia.Bot.Objects.Constants.Responses;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class ItemView : KaiaItemContentView
    {
        public ItemView(ItemsPaginated? From, CommandContext Context, KaiaInventoryItem Item, IEmote BuyItemEmote, IEmote InteractWithItemEmote, bool CanGoBack) : base(From, Context, CanGoBack)
        {
            this.Item = Item;
            this.BuyItemEmote = BuyItemEmote;
            this.InteractWithItemEmote = InteractWithItemEmote;
            this.Refreshed = false;
        }
        public KaiaInventoryItem Item { get; }
        public IEmote BuyItemEmote { get; }
        public IEmote InteractWithItemEmote { get; }
        public ulong BId { get; } = IdGenerator.CreateNewId();
        public ulong IId { get; } = IdGenerator.CreateNewId();
        public string BuyId => $"{this.Item.DisplayName}-{this.BId}";
        public string InteractId => $"{this.Item.DisplayName}-{this.IId}";
        private bool Refreshed { get; set; }
        public DateRateLimiter RateLimiter { get; } = new(DataStores.RateLimitsStore, "Kaia Item", TimeSpan.FromSeconds(8), 3, TimeSpan.FromSeconds(4));
        public DateRateLimiter SecondaryRateLimiter { get; } = new(DataStores.RateLimitsStore, "Secondary Kaia Item", TimeSpan.FromSeconds(2));

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = (await this.GetDefaultComponents())
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

            return CB;
        }

        public override Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U)
        {
            KaiaPathEmbed Em = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.StoreOrShop, this.Item.DisplayName);

            Em.WithField($"[{Strings.Economy.CurrencyEmote} `{this.Item.Cost}`] {this.Item.DisplayName}  {this.Item.DisplayEmote}", this.Item.Description);
            Em.WithField("your balance", $"{Strings.Economy.CurrencyEmote} `{U.Settings.Inventory.Petals}`{(this.Refreshed ? "- balances may go up due to passive income from books every time it refreshes." : "")}");
            Em.WithField($"number of {this.Item.DisplayName}s owned", $"`{U.Settings.Inventory.Items.Count(I => I.DisplayName == this.Item.DisplayName)}`");
            this.Refreshed = true;
            return Task.FromResult(Em);
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbed E = await this.GetEmbedAsync(U);
            ComponentBuilder Com = await this.GetComponentsAsync(U);
            _ = await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
            this.Context.Reference.Client.ButtonExecuted += this.ButtonExecutedAsync;
        }

        private async Task ButtonExecutedAsync(SocketMessageComponent Arg)
        {
            if (Arg.IsValidToken && (Arg.Data.CustomId == this.BuyId || Arg.Data.CustomId == this.InteractId) && Arg.User.Id == this.Context.UserContext.User.Id)
            {
                if(await this.RateLimiter.CheckIfPassesAsync(Arg.User.Id) || Arg.Data.CustomId == this.BuyId)
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
                else if(await this.SecondaryRateLimiter.CheckIfPassesAsync(Arg.User.Id) && this.Context.UserContext.IsValidToken)
                {
                    await Responses.PipeErrors(this.Context, EmbedDefaults.RateLimitEmbed);
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.Client.ButtonExecuted -= this.ButtonExecutedAsync;
        }
    }
}
