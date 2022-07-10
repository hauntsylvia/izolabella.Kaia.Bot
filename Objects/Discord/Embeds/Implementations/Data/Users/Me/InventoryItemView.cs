using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Util.RateLimits.Limiters;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeViews
{
    public class InventoryItemView : KaiaItemContentView
    {
        public InventoryItemView(MeInventoryView From, KaiaInventoryItem Item, CommandContext Context, KaiaUser U) : base(From, Context, true)
        {
            this.From = From;
            this.Item = Item;
            this.U = U;
            Interact = new(Context, "Interact", Emotes.Counting.InteractItem);
            PutUpForSaleButton = new(Context, "Sell", Emotes.Counting.SellItem);

            Interact.OnButtonPush += InteractAsync;
            PutUpForSaleButton.OnButtonPush += SellAsync;

            Interact.OnButtonPush += ButtonPressed;
            PutUpForSaleButton.OnButtonPush += ButtonPressed;
        }

        public ItemCreateSaleListing? ListingInteraction { get; set; }

        public MeInventoryView? From { get; }

        public KaiaInventoryItem? Item { get; private set; }

        public KaiaUser U { get; }

        public KaiaButton Interact { get; }

        public KaiaButton PutUpForSaleButton { get; }

        public DateRateLimiter RateLimiter { get; } = new(DataStores.RateLimitsStore, "Kaia Inv Item", TimeSpan.FromSeconds(8), 6, TimeSpan.FromSeconds(4));

        private async Task ButtonPressed(SocketMessageComponent Arg, KaiaUser U)
        {
            if (await RateLimiter.PassesAsync(Arg.User.Id))
            {
                if (Arg.Data.CustomId != PutUpForSaleButton.Id)
                {
                    await U.SaveAsync();
                    KaiaPathEmbedRefreshable E = await GetEmbedAsync(U);
                    ComponentBuilder Com = await GetComponentsAsync(U);
                    await Arg.UpdateAsync(A =>
                    {
                        A.Embed = E.Build();
                        A.Components = Com.Build();
                    });
                }
                else if (!Arg.HasResponded)
                {
                    await Arg.DeferAsync(true);
                }
            }
        }

        private async Task SellAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (ListingInteraction == null && Item != null)
            {
                ListingInteraction = new ItemCreateSaleListing(this, Context, new(new() { Item }, this.U, Item.MarketCost));
            }
            else if (ListingInteraction != null)
            {
                ListingInteraction.Dispose();
            }

            if (ListingInteraction != null)
            {
                await ListingInteraction.StartAsync(U);
            }
        }

        private async Task InteractAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (Item != null && await U.Settings.Inventory.GetItemOfId(Item.Id) != null)
            {
                await Context.UserContext.FollowupAsync(embed: new InteractWithItemEmbed(Item, U).Build());
                Item = await U.Settings.Inventory.GetItemOfDisplayName(Item);
            }
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            KaiaPathEmbedRefreshable V = Item == null ? new SingleItemNotFound() : new InventoryItemViewRaw(Item);
            await V.RefreshAsync();
            return V;
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = await GetDefaultComponents();
            CB.WithButton(Interact.WithDisabled(!await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || (!Item?.CanInteractWithDirectly ?? true)));
            if (Item != null && Item.UsersCanSellThis)
            {
                CB.WithButton(PutUpForSaleButton.WithDisabled(!await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || !Item.UsersCanSellThis));
            }
            return CB;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            Embed E = (await GetEmbedAsync(U)).Build();
            MessageComponent Com = (await GetComponentsAsync(U)).Build();
            if (!Context.UserContext.HasResponded)
            {
                await Context.UserContext.RespondAsync(components: Com, embed: E);
            }
            else
            {
                await Context.UserContext.ModifyOriginalResponseAsync(M =>
                {
                    M.Content = Strings.EmbedStrings.Empty;
                    M.Components = Com;
                    M.Embed = E;
                });
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Interact.Dispose();
            PutUpForSaleButton.Dispose();
        }
    }
}
