using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Me;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemCreateSaleListing : KaiaItemContentView
    {
        public ItemCreateSaleListing(ItemView PreviousPage, CommandContext Context, SaleListing FromListing) : base(PreviousPage, Context, true)
        {
            KaiaUser U = new(Context.UserContext.User.Id);
            this.Listing = new(new(), U, FromListing.CostPerItem);
            foreach (KaiaInventoryItem Item in FromListing.Items.ToList())
            {
                KaiaInventoryItem? RelevantItem = U.Settings.Inventory.GetItemOfDisplayName(Item).Result;
                if (RelevantItem != null)
                {
                    this.Listing.Items.Add(RelevantItem);
                }
            }
            PreviousPage.ListingInteraction = this;
            this.PreviousPageStore = PreviousPage;
            this.SubmitButton = new(this.Context, "Submit", Emotes.Counting.SellItem);
            this.AddOneMoreButton = new(this.Context, "1", Emotes.Counting.Add);
            this.RemoveOneMoreButton = new(this.Context, "1", Emotes.Counting.Sub);
            Context.Reference.MessageReceived += this.MessageReceivedAsync;
        }

        public ItemCreateSaleListing(InventoryItemView PreviousPage, CommandContext Context, SaleListing FromListing) : base(PreviousPage, Context, true)
        {
            KaiaUser U = new(Context.UserContext.User.Id);
            this.Listing = new(new(), U, FromListing.CostPerItem);
            foreach (KaiaInventoryItem Item in FromListing.Items.ToList())
            {
                KaiaInventoryItem? RelevantItem = U.Settings.Inventory.GetItemOfDisplayName(Item).Result;
                if (RelevantItem != null)
                {
                    this.Listing.Items.Add(RelevantItem);
                }
            }
            PreviousPage.ListingInteraction = this;
            this.PreviousPageInventory = PreviousPage;
            this.SubmitButton = new(this.Context, "Submit", Emotes.Counting.SellItem);
            this.AddOneMoreButton = new(this.Context, "1", Emotes.Counting.Add);
            this.RemoveOneMoreButton = new(this.Context, "1", Emotes.Counting.Sub);
            Context.Reference.MessageReceived += this.MessageReceivedAsync;
        }

        #region properties

        public SaleListing Listing { get; }

        public ItemView? PreviousPageStore { get; }

        public InventoryItemView? PreviousPageInventory { get; }

        public KaiaButton SubmitButton { get; private set; }

        public KaiaButton AddOneMoreButton { get; private set; }

        public KaiaButton RemoveOneMoreButton { get; private set; }

        #endregion

        #region button events

        private async Task AddOneAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            IEnumerable<KaiaInventoryItem> ItemsMinusExistingIdsInThisListing = U.Settings.Inventory.Items
                .Where(I => I.UsersCanSellThis && !this.Listing.Items.Any(ListingItem => ListingItem.Id == I.Id) && I.DisplayName == this.Listing.Items.First().DisplayName);
            KaiaInventoryItem? RelevantItem = await U.Settings.Inventory.GetItemOfId(ItemsMinusExistingIdsInThisListing.FirstOrDefault()?.Id ?? 0);
            if (RelevantItem != null)
            {
                this.Listing.Items.Add(RelevantItem);
            }
        }

        private Task RemoveOneAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (this.Listing.Items.Count > 0)
            {
                this.Listing.Items.RemoveAt(0);
            }
            return Task.CompletedTask;
        }

        private async Task SubmitAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            await Arg.DeferAsync();
            await this.Listing.StartSellingAsync();
            this.Dispose();
            if (this.PreviousPageStore != null)
            {
                this.PreviousPageStore.Dispose();
            }
            if (this.PreviousPageInventory != null)
            {
                this.PreviousPageInventory.Dispose();
            }
            if (this.PreviousPageStore?.From != null || this.PreviousPageInventory?.From != null)
            {
                this.PreviousPageStore?.From?.Dispose();
                if (this.PreviousPageStore != null)
                {
                    await new ItemsPaginated(this.Context, this.PreviousPageStore?.From?.FilterBy, this.PreviousPageStore?.From?.IncludeUserListings ?? true, this.PreviousPageStore?.From?.ChunkSize ?? 2).StartAsync();
                }
                else
                {
                    await new MeInventoryView(U, this.Context, this.PreviousPageInventory?.From?.InventoryChunkSize ?? 4).StartAsync();
                }
            }
            else
            {
                await new ItemsPaginated(this.Context).StartAsync();
            }
        }

        private async Task UpdateEmbedAsync(SocketMessageComponent? Arg, KaiaUser U)
        {
            if (Arg == null || !(Arg.Data.CustomId == this.SubmitButton.Id && this.PreviousPageElse != null))
            {
                await U.SaveAsync();
                KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
                ComponentBuilder Com = await this.GetComponentsAsync(U);
                if (Arg != null)
                {
                    await Arg.UpdateAsync(C =>
                    {
                        C.Embed = E.Build();
                        C.Components = Com.Build();
                        C.Content = null;
                    });
                }
                else
                {
                    await this.Context.UserContext.ModifyOriginalResponseAsync(C =>
                    {
                        C.Embed = E.Build();
                        C.Components = Com.Build();
                        C.Content = null;
                    });
                }
            }
        }

        #endregion

        #region other events

        private async Task MessageReceivedAsync(SocketMessage Arg)
        {
            if (Arg.Author.Id == this.Context.UserContext.User.Id && double.TryParse(Arg.Content, out double NewPricePerItem))
            {
                this.Listing.CostPerItem = NewPricePerItem;
                await this.UpdateEmbedAsync(null, new(Arg.Author.Id));
            }
        }

        #endregion

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = (await this.GetDefaultComponents())
                .WithButton(this.SubmitButton.WithDisabled(
                    this.Listing.CostPerItem <= 0 || !this.Listing.Items.All(I => I.UsersCanSellThis) || (await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>()).Any(S => S.ListerId == this.Listing.ListerId)))
                .WithButton(this.AddOneMoreButton.WithDisabled(U.Settings.Inventory.GetItemsOfDisplayNameFromItem(this.Listing.Items.First()).Result.Count() <= this.Listing.Items.Count))
                .WithButton(this.RemoveOneMoreButton.WithDisabled(this.Listing.Items.Count <= 1));
            return CB;
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

            this.SubmitButton.OnButtonPush += this.SubmitAsync;
            this.AddOneMoreButton.OnButtonPush += this.AddOneAsync;
            this.RemoveOneMoreButton.OnButtonPush += this.RemoveOneAsync;

            this.SubmitButton.OnButtonPush += this.UpdateEmbedAsync;
            this.AddOneMoreButton.OnButtonPush += this.UpdateEmbedAsync;
            this.RemoveOneMoreButton.OnButtonPush += this.UpdateEmbedAsync;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            ItemCreateSaleListingRaw Em = new(this.Listing);
            await Em.RefreshAsync();
            return Em;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.MessageReceived -= this.MessageReceivedAsync;
            this.SubmitButton.Dispose();
            this.AddOneMoreButton.Dispose();
            this.RemoveOneMoreButton.Dispose();
        }
    }
}
