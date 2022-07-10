using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Constants.Responses;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Util.RateLimits.Limiters;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemView : KaiaItemContentView
    {
        public ItemView(ItemsPaginated? From, CommandContext Context, SaleListing Listing, IEmote BuyItemEmote, IEmote InteractWithItemEmote, IEmote PutUpForSaleEmote, bool CanGoBack) : base(From, Context, CanGoBack)
        {
            this.From = From;
            this.Listing = Listing;
            BuyButton = new(Context, "Buy", BuyItemEmote);
            InteractWithButton = new(Context, "Interact", InteractWithItemEmote);
            PutUpForSaleButton = new(Context, "Sell", PutUpForSaleEmote);
            RemoveListingButton = new(Context, "Remove", Emotes.Counting.Cancel);
            Refreshed = false;
            BuyButton.OnButtonPush += BuyAsync;
            InteractWithButton.OnButtonPush += InteractAsync;
            PutUpForSaleButton.OnButtonPush += SellAsync;
            RemoveListingButton.OnButtonPush += RemoveListingAsync;

            BuyButton.OnButtonPush += UpdateEmbedAsync;
            InteractWithButton.OnButtonPush += UpdateEmbedAsync;
            PutUpForSaleButton.OnButtonPush += UpdateEmbedAsync;
            RemoveListingButton.OnButtonPush += UpdateEmbedAsync;
        }

        #region properties

        public ItemsPaginated? From { get; }

        public SaleListing Listing { get; }

        public KaiaButton BuyButton { get; }

        public KaiaButton InteractWithButton { get; }

        public KaiaButton PutUpForSaleButton { get; }

        public KaiaButton RemoveListingButton { get; }

        private bool Refreshed { get; set; }

        public DateRateLimiter RateLimiter { get; } = new(DataStores.RateLimitsStore, "Kaia Item", TimeSpan.FromSeconds(8), 6, TimeSpan.FromSeconds(4));

        public DateRateLimiter SecondaryRateLimiter { get; } = new(DataStores.RateLimitsStore, "Secondary Kaia Item", TimeSpan.FromSeconds(2));

        public ItemCreateSaleListing? ListingInteraction { get; set; }

        #endregion

        #region button events

        private async Task RemoveListingAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            if (UserWhoPressed.Id == Listing.ListerId)
            {
                await Listing.StopSellingAsync();
                Dispose();
                await ForceBackAsync(Context);
            }
        }

        private async Task SellAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (ListingInteraction == null)
            {
                ListingInteraction = new ItemCreateSaleListing(this, Context, Listing);
            }
            else
            {
                ListingInteraction.Dispose();
            }
            await ListingInteraction.StartAsync(U);
        }

        private async Task BuyAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            await Listing.UserBoughtAsync(U);
        }

        private async Task InteractAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            KaiaInventoryItem? I = await U.Settings.Inventory.GetItemOfDisplayName(Listing.Items.FirstOrDefault());
            if (I != null)
            {
                await Context.UserContext.FollowupAsync(embed: new InteractWithItemEmbed(I, U).Build());
            }
        }

        private async Task UpdateEmbedAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (await RateLimiter.PassesAsync(Arg.User.Id))
            {
                if (Arg.Data.CustomId != PutUpForSaleButton.Id && Arg.Data.CustomId != RemoveListingButton.Id)
                {
                    await U.SaveAsync();
                    KaiaPathEmbedRefreshable E = await GetEmbedAsync(U);
                    ComponentBuilder Com = await GetComponentsAsync(U);
                    if (Listing.Items.Count <= 0)
                    {
                        await Arg.DeferAsync();
                        Dispose();
                        if (From != null)
                        {
                            From.Dispose();
                            await new ItemsPaginated(Context, From.FilterBy, From.IncludeUserListings, From.ChunkSize).StartAsync();
                        }
                        else
                        {
                            await new ItemsPaginated(Context).StartAsync();
                        }
                    }
                    else
                    {
                        await Arg.UpdateAsync(A =>
                        {
                            A.Embed = E.Build();
                            A.Components = Com.Build();
                        });
                    }
                }
                else if (!Arg.HasResponded)
                {
                    await Arg.DeferAsync(true);
                }
            }
            else if (await SecondaryRateLimiter.PassesAsync(Arg.User.Id) && Context.UserContext.IsValidToken)
            {
                await Responses.PipeErrors(Context, new RateLimited());
            }
        }

        #endregion

        #region get message stuff

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            KaiaInventoryItem? Item = Listing.Items.FirstOrDefault();
            ComponentBuilder CB = (await GetDefaultComponents())
                .WithButton(BuyButton.WithDisabled(U.Settings.Inventory.Petals < Listing.CostPerItem || Listing.Items.Count <= 0));
            if (Listing.ListerId == null)
            {
                CB.WithButton(InteractWithButton.WithDisabled(Item == null || !await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || !Item.CanInteractWithDirectly));
            }
            if (Listing.Items.All(I => I.UsersCanSellThis) && Item != null)
            {
                CB.WithButton(PutUpForSaleButton.WithDisabled(!await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || !Item.UsersCanSellThis));
            }
            if (Listing.ListerId == U.Id)
            {
                CB.WithButton(RemoveListingButton.WithDisabled(false));
            }
            return CB;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            KaiaInventoryItem? Item = Listing.Items.FirstOrDefault();
            KaiaPathEmbedRefreshable Embed = Item != null ? new ItemRawView(Context, Item, U, Listing, Refreshed) : new SingleItemNotFound();
            Refreshed = true;
            await Embed.RefreshAsync();
            return Embed;
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

        #endregion

        #region dispose

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            BuyButton.Dispose();
            InteractWithButton.Dispose();
            PutUpForSaleButton.Dispose();
        }

        #endregion
    }
}
