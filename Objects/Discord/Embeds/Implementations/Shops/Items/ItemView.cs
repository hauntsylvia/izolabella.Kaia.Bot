using izolabella.Util;
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
            this.BuyItemEmote = BuyItemEmote;
            this.InteractWithItemEmote = InteractWithItemEmote;
            this.PutUpForSaleEmote = PutUpForSaleEmote;
            this.Refreshed = false;
        }

        public ItemsPaginated? From { get; }
        public SaleListing Listing { get; }
        public IEmote BuyItemEmote { get; }
        public IEmote InteractWithItemEmote { get; }
        public IEmote PutUpForSaleEmote { get; }
        public ulong BId { get; } = IdGenerator.CreateNewId();
        public ulong IId { get; } = IdGenerator.CreateNewId();
        public ulong SId { get; } = IdGenerator.CreateNewId();
        public string BuyId => $"{this.Listing.Id}-{this.BId}";
        public string InteractId => $"{this.Listing.Id}-{this.IId}";
        public string PutUpForSaleId => $"{this.Listing.Id}-{this.SId}";
        private bool Refreshed { get; set; }
        public DateRateLimiter RateLimiter { get; } = new(DataStores.RateLimitsStore, "Kaia Item", TimeSpan.FromSeconds(8), 3, TimeSpan.FromSeconds(4));
        public DateRateLimiter SecondaryRateLimiter { get; } = new(DataStores.RateLimitsStore, "Secondary Kaia Item", TimeSpan.FromSeconds(2));
        public ItemCreateSaleListing? ListingInteraction { get; set; }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            KaiaInventoryItem? Item = this.Listing.Items.FirstOrDefault();
            ComponentBuilder CB = (await this.GetDefaultComponents())
                .WithButton("Buy",
                           this.BuyId,
                           ButtonStyle.Secondary,
                           this.BuyItemEmote,
                           disabled: U.Settings.Inventory.Petals < this.Listing.CostPerItem || this.Listing.Items.Count <= 0);
            if (this.Listing.Items.All(I => I.UsersCanSellThis) && Item != null)
            {
                CB.WithButton("Sell", this.PutUpForSaleId, ButtonStyle.Secondary, this.PutUpForSaleEmote, disabled: !await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || !Item.UsersCanSellThis);
            }
            if(this.Listing.ListerId == null)
            {
                CB.WithButton("Interact",
                           this.InteractId,
                           ButtonStyle.Secondary,
                           this.InteractWithItemEmote,
                           disabled: Item == null || !await U.Settings.Inventory.ItemOfDisplayNameExists(Item) || !Item.CanInteractWithDirectly);
            }
            return CB;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            KaiaInventoryItem? Item = this.Listing.Items.FirstOrDefault();
            KaiaPathEmbedRefreshable Em = Item != null ? new ItemRawView(this.Context, Item, U, this.Listing, this.Refreshed) : new SingleItemNotFound();
            this.Refreshed = true;
            return Em;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
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
            if (Arg.IsValidToken
                && (Arg.Data.CustomId == this.BuyId
                    || Arg.Data.CustomId == this.InteractId
                    || Arg.Data.CustomId == this.PutUpForSaleId)
                && Arg.User.Id == this.Context.UserContext.User.Id)
            {
                if (await this.RateLimiter.CheckIfPassesAsync(Arg.User.Id) || Arg.Data.CustomId == this.BuyId || Arg.Data.CustomId == this.PutUpForSaleId)
                {
                    KaiaUser U = new(Arg.User.Id);
                    KaiaInventoryItem? Item = this.Listing.Items.FirstOrDefault();
                    if (Item != null && Arg.Data.CustomId == this.BuyId && U.Settings.Inventory.Petals >= this.Listing.CostPerItem)
                    {
                        await this.Listing.UserBoughtAsync(U);
                    }
                    else if (Item != null && Arg.Data.CustomId == this.InteractId && await U.Settings.Inventory.ItemOfDisplayNameExists(Item))
                    {
                        await U.Settings.Inventory.RemoveItemOfNameAsync(Item);
                        await Item.UserInteractAsync(this.Context, U);
                    }
                    else if (Item != null && Arg.Data.CustomId == this.PutUpForSaleId)
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
                    if (Arg.Data.CustomId != this.PutUpForSaleId)
                    {
                        await U.SaveAsync();
                        KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
                        ComponentBuilder Com = await this.GetComponentsAsync(U);
                        if(this.Listing.Items.Count <= 0)
                        {
                            await Arg.DeferAsync();
                            this.Dispose();
                            if(this.From != null)
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
                    else
                    {
                        await Arg.DeferAsync(true);
                    }
                }
                else if (await this.SecondaryRateLimiter.CheckIfPassesAsync(Arg.User.Id) && this.Context.UserContext.IsValidToken)
                {
                    await Responses.PipeErrors(this.Context, new RateLimited());
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
