using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeViews;
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
            Listing = new(new(), U, FromListing.CostPerItem);
            foreach (KaiaInventoryItem Item in FromListing.Items.ToList())
            {
                KaiaInventoryItem? RelevantItem = U.Settings.Inventory.GetItemOfDisplayName(Item).Result;
                if (RelevantItem != null)
                {
                    Listing.Items.Add(RelevantItem);
                }
            }
            PreviousPage.ListingInteraction = this;
            PreviousPageStore = PreviousPage;
            SubmitButton = new(this.Context, "Submit", Emotes.Counting.SellItem);
            AddOneMoreButton = new(this.Context, "1", Emotes.Counting.Add);
            RemoveOneMoreButton = new(this.Context, "1", Emotes.Counting.Sub);
            Context.Reference.MessageReceived += MessageReceivedAsync;
        }

        public ItemCreateSaleListing(InventoryItemView PreviousPage, CommandContext Context, SaleListing FromListing) : base(PreviousPage, Context, true)
        {
            KaiaUser U = new(Context.UserContext.User.Id);
            Listing = new(new(), U, FromListing.CostPerItem);
            foreach (KaiaInventoryItem Item in FromListing.Items.ToList())
            {
                KaiaInventoryItem? RelevantItem = U.Settings.Inventory.GetItemOfDisplayName(Item).Result;
                if (RelevantItem != null)
                {
                    Listing.Items.Add(RelevantItem);
                }
            }
            PreviousPage.ListingInteraction = this;
            PreviousPageInventory = PreviousPage;
            SubmitButton = new(this.Context, "Submit", Emotes.Counting.SellItem);
            AddOneMoreButton = new(this.Context, "1", Emotes.Counting.Add);
            RemoveOneMoreButton = new(this.Context, "1", Emotes.Counting.Sub);
            Context.Reference.MessageReceived += MessageReceivedAsync;
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
                .Where(I => I.UsersCanSellThis && !Listing.Items.Any(ListingItem => ListingItem.Id == I.Id) && I.DisplayName == Listing.Items.First().DisplayName);
            KaiaInventoryItem? RelevantItem = await U.Settings.Inventory.GetItemOfId(ItemsMinusExistingIdsInThisListing.FirstOrDefault()?.Id ?? 0);
            if (RelevantItem != null)
            {
                Listing.Items.Add(RelevantItem);
            }
        }

        private Task RemoveOneAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (Listing.Items.Count > 0)
            {
                Listing.Items.RemoveAt(0);
            }
            return Task.CompletedTask;
        }

        private async Task SubmitAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            await Arg.DeferAsync();
            await Listing.StartSellingAsync();
            Dispose();
            if (PreviousPageStore != null)
            {
                PreviousPageStore.Dispose();
            }
            if (PreviousPageInventory != null)
            {
                PreviousPageInventory.Dispose();
            }
            if (PreviousPageStore?.From != null || PreviousPageInventory?.From != null)
            {
                PreviousPageStore?.From?.Dispose();
                if (PreviousPageStore != null)
                {
                    await new ItemsPaginated(Context, PreviousPageStore?.From?.FilterBy, PreviousPageStore?.From?.IncludeUserListings ?? true, PreviousPageStore?.From?.ChunkSize ?? 2).StartAsync();
                }
                else
                {
                    await new MeInventoryView(U, Context, PreviousPageInventory?.From?.InventoryChunkSize ?? 4).StartAsync();
                }
            }
            else
            {
                await new ItemsPaginated(Context).StartAsync();
            }
        }

        private async Task UpdateEmbedAsync(SocketMessageComponent? Arg, KaiaUser U)
        {
            if (Arg == null || !(Arg.Data.CustomId == SubmitButton.Id && PreviousPageElse != null))
            {
                await U.SaveAsync();
                KaiaPathEmbedRefreshable E = await GetEmbedAsync(U);
                ComponentBuilder Com = await GetComponentsAsync(U);
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
                    await Context.UserContext.ModifyOriginalResponseAsync(C =>
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
            if (Arg.Author.Id == Context.UserContext.User.Id && double.TryParse(Arg.Content, out double NewPricePerItem))
            {
                Listing.CostPerItem = NewPricePerItem;
                await UpdateEmbedAsync(null, new(Arg.Author.Id));
            }
        }

        #endregion

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = (await GetDefaultComponents())
                .WithButton(SubmitButton.WithDisabled(
                    Listing.CostPerItem <= 0 || !Listing.Items.All(I => I.UsersCanSellThis) || (await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>()).Any(S => S.ListerId == Listing.ListerId)))
                .WithButton(AddOneMoreButton.WithDisabled(U.Settings.Inventory.GetItemsOfDisplayNameFromItem(Listing.Items.First()).Result.Count() <= Listing.Items.Count))
                .WithButton(RemoveOneMoreButton.WithDisabled(Listing.Items.Count <= 1));
            return CB;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!Context.UserContext.HasResponded)
            {
                await Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await GetEmbedAsync(U);
            ComponentBuilder Com = await GetComponentsAsync(U);
            _ = await Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });

            SubmitButton.OnButtonPush += SubmitAsync;
            AddOneMoreButton.OnButtonPush += AddOneAsync;
            RemoveOneMoreButton.OnButtonPush += RemoveOneAsync;

            SubmitButton.OnButtonPush += UpdateEmbedAsync;
            AddOneMoreButton.OnButtonPush += UpdateEmbedAsync;
            RemoveOneMoreButton.OnButtonPush += UpdateEmbedAsync;
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            ItemCreateSaleListingRaw Em = new(Listing);
            await Em.RefreshAsync();
            return Em;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Context.Reference.MessageReceived -= MessageReceivedAsync;
            SubmitButton.Dispose();
            AddOneMoreButton.Dispose();
            RemoveOneMoreButton.Dispose();
        }
    }
}
