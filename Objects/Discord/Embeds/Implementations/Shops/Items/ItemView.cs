﻿using izolabella.Util;
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
            this.Refreshed = false;

            this.BuyButton.OnButtonPush += this.BuyAsync;
            this.InteractWithButton.OnButtonPush += this.InteractAsync;
            this.PutUpForSaleButton.OnButtonPush += this.SellAsync;

            this.BuyButton.OnButtonPush += this.UpdateEmbedAsync;
            this.InteractWithButton.OnButtonPush += this.UpdateEmbedAsync;
        }

        #region properties

        public ItemsPaginated? From { get; }

        public SaleListing Listing { get; }

        public KaiaButton BuyButton { get; }

        public KaiaButton InteractWithButton { get; }

        public KaiaButton PutUpForSaleButton { get; }

        private bool Refreshed { get; set; }

        public DateRateLimiter RateLimiter { get; } = new(DataStores.RateLimitsStore, "Kaia Item", TimeSpan.FromSeconds(8), 3, TimeSpan.FromSeconds(4));
        
        public DateRateLimiter SecondaryRateLimiter { get; } = new(DataStores.RateLimitsStore, "Secondary Kaia Item", TimeSpan.FromSeconds(2));
        
        public ItemCreateSaleListing? ListingInteraction { get; set; }

        #endregion

        #region button events
        private async Task SellAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            this.Dispose();
            if (this.ListingInteraction == null)
            {
                await new ItemCreateSaleListing(this, this.Context, this.Listing).StartAsync(U);
            }
            else
            {
                this.ListingInteraction.Dispose();
                await this.ListingInteraction.StartAsync(U);
            }
        }

        private async Task BuyAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            await this.Listing.UserBoughtAsync(U);
        }

        private async Task InteractAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            KaiaInventoryItem? Item = this.Listing.Items.FirstOrDefault();
            if(Item != null && await U.Settings.Inventory.ItemOfDisplayNameExists(Item))
            {
                await U.Settings.Inventory.RemoveItemOfNameAsync(Item);
                await Item.UserInteractAsync(this.Context, U);
            }
        }

        private async Task UpdateEmbedAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (await this.RateLimiter.CheckIfPassesAsync(Arg.User.Id) && Arg.Data.CustomId != this.PutUpForSaleButton.Id)
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
            else if (await this.SecondaryRateLimiter.CheckIfPassesAsync(Arg.User.Id) && this.Context.UserContext.IsValidToken)
            {
                await Responses.PipeErrors(this.Context, new RateLimited());
            }
            else
            {
                await Arg.DeferAsync(true);
            }
        }
        #endregion

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
            return CB;
        }

        #region overrides

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
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
            ComponentBuilder Com = await this.GetComponentsAsync(U);
            await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
        }

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