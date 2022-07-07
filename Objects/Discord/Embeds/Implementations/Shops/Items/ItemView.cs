using izolabella.Util.RateLimits.Limiters;
using Kaia.Bot.Objects.Constants.Responses;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemView : KaiaItemContentView
    {
        public ItemView(ItemsPaginated? From, CommandContext Context, SaleListing Listing, IEmote BuyItemEmote, IEmote InteractWithItemEmote, IEmote PutUpForSaleEmote, bool CanGoBack) : base(From, Context, CanGoBack)
        {
            this.From = From;
            this.Listing = Listing;
            this.BuyButton = new(Context, "Buy", BuyItemEmote);
            this.InteractWithButton = new(Context, "Interact", InteractWithItemEmote);
            this.PutUpForSaleButton = new(Context, "Sell", PutUpForSaleEmote);
            this.RemoveListingButton = new(Context, "Remove", Emotes.Counting.Cancel);
            this.Refreshed = false;
            this.BuyButton.OnButtonPush += this.BuyAsync;
            this.InteractWithButton.OnButtonPush += this.InteractAsync;
            this.PutUpForSaleButton.OnButtonPush += this.SellAsync;
            this.RemoveListingButton.OnButtonPush += this.RemoveListingAsync;

            this.BuyButton.OnButtonPush += this.UpdateEmbedAsync;
            this.InteractWithButton.OnButtonPush += this.UpdateEmbedAsync;
            this.PutUpForSaleButton.OnButtonPush += this.UpdateEmbedAsync;
            this.RemoveListingButton.OnButtonPush += this.UpdateEmbedAsync;
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
            if(UserWhoPressed.Id == this.Listing.ListerId)
            {
                await this.Listing.StopSellingAsync();
                this.Dispose();
                await this.ForceBackAsync(this.Context);
            }
        }

        private async Task SellAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (this.ListingInteraction == null)
            {
                this.ListingInteraction = new ItemCreateSaleListing(this, this.Context, this.Listing);
            }
            else
            {
                this.ListingInteraction.Dispose();
            }
            await this.ListingInteraction.StartAsync(U);
        }

        private async Task BuyAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            await this.Listing.UserBoughtAsync(U);
        }

        private async Task InteractAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            KaiaInventoryItem? I = await U.Settings.Inventory.GetItemOfDisplayName(this.Listing.Items.FirstOrDefault());
            if (I != null)
            {
                await this.Context.UserContext.FollowupAsync(embed: new InteractWithItemEmbed(I, U).Build());
            }
        }

        private async Task UpdateEmbedAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (await this.RateLimiter.PassesAsync(Arg.User.Id))
            {
                if(Arg.Data.CustomId != this.PutUpForSaleButton.Id && Arg.Data.CustomId != this.RemoveListingButton.Id)
                {
                    await U.SaveAsync();
                    KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
                    ComponentBuilder Com = await this.GetComponentsAsync(U);
                    if (this.Listing.Items.Count <= 0)
                    {
                        await Arg.DeferAsync();
                        this.Dispose();
                        if (this.From != null)
                        {
                            this.From.Dispose();
                            await new ItemsPaginated(this.Context, this.From.FilterBy, this.From.IncludeUserListings, this.From.ChunkSize).StartAsync();
                        }
                        else
                        {
                            await new ItemsPaginated(this.Context).StartAsync();
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
                else if(!Arg.HasResponded)
                {
                    await Arg.DeferAsync(true);
                }
            }
            else if (await this.SecondaryRateLimiter.PassesAsync(Arg.User.Id) && this.Context.UserContext.IsValidToken)
            {
                await Responses.PipeErrors(this.Context, new RateLimited());
            }
        }

        #endregion

        #region get message stuff

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            KaiaInventoryItem? Item = this.Listing.Items.FirstOrDefault();
            ComponentBuilder CB = (await this.GetDefaultComponents())
                .WithButton(this.BuyButton.WithDisabled(U.Settings.Inventory.Petals < this.Listing.CostPerItem || this.Listing.Items.Count <= 0));
            if (this.Listing.ListerId == null)
            {
                CB.WithButton(this.InteractWithButton.WithDisabled(Item == null || !await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || !Item.CanInteractWithDirectly));
            }
            if (this.Listing.Items.All(I => I.UsersCanSellThis) && Item != null)
            {
                CB.WithButton(this.PutUpForSaleButton.WithDisabled(!await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || !Item.UsersCanSellThis));
            }
            if(this.Listing.ListerId == U.Id)
            {
                CB.WithButton(this.RemoveListingButton.WithDisabled(false));
            }
            return CB;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            KaiaInventoryItem? Item = this.Listing.Items.FirstOrDefault();
            KaiaPathEmbedRefreshable Embed = Item != null ? new ItemRawView(this.Context, Item, U, this.Listing, this.Refreshed) : new SingleItemNotFound();
            this.Refreshed = true;
            await Embed.RefreshAsync();
            return Embed;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            Embed E = (await this.GetEmbedAsync(U)).Build();
            MessageComponent Com = (await this.GetComponentsAsync(U)).Build();
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(components: Com, embed: E);
            }
            else
            {
                await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
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
            this.BuyButton.Dispose();
            this.InteractWithButton.Dispose();
            this.PutUpForSaleButton.Dispose();
        }

        #endregion
    }
}
