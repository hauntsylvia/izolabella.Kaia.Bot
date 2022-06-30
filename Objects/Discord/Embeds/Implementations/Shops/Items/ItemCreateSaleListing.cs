﻿using izolabella.Util;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class ItemCreateSaleListing : KaiaItemContentView
    {
        public ItemCreateSaleListing(ItemView PreviousPage, CommandContext Context, SaleListing FromListing) : base(PreviousPage, Context, true)
        {
            KaiaUser U = new(Context.UserContext.User.Id);
            this.Listing = new(new(), U, FromListing.CostPerItem);
            foreach(KaiaInventoryItem Item in FromListing.Items.ToList())
            {
                KaiaInventoryItem? RelevantItem = U.Settings.Inventory.GetItemOfDisplayName(Item).Result;
                if (RelevantItem != null)
                {
                    this.Listing.Items.Add(RelevantItem);
                }
            }
            PreviousPage.ListingInteraction = this;
            this.PreviousPage = PreviousPage;
            this.SubmitButton = new(Context, "Submit", Emotes.Counting.SellItem);
            this.AddOneMoreButton = new(Context, "Add", Emotes.Counting.Add);
        }

        #region properties

        public SaleListing Listing { get; }

        public ItemView PreviousPage { get; }

        public KaiaButton SubmitButton { get; }

        public KaiaButton AddOneMoreButton { get; }

        #endregion

        #region button events

        private async Task AddOneAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            IEnumerable<KaiaInventoryItem> ItemsMinusExistingIdsInThisListing = U.Settings.Inventory.Items.Where(I => I.UsersCanSellThis && !this.Listing.Items.Any(ListingItem => ListingItem.Id == I.Id));
            KaiaInventoryItem? RelevantItem = await U.Settings.Inventory.GetItemOfId(ItemsMinusExistingIdsInThisListing.FirstOrDefault()?.Id ?? 0);
            if(RelevantItem != null)
            {
                this.Listing.Items.Add(RelevantItem);
            }
        }

        private async Task SubmitAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            await Arg.DeferAsync();
            await this.Listing.StartSellingAsync();
            this.Dispose();
            this.PreviousPage.Dispose();
            if (this.PreviousPage.From != null)
            {
                this.PreviousPage.From.Dispose();
                await new ItemsPaginated(this.Context, this.PreviousPage.From.FilterBy, this.PreviousPage.From.IncludeUserListings, this.PreviousPage.From.ChunkSize).StartAsync();
            }
            else
            {
                await new ItemsPaginated(this.Context).StartAsync();
            }
        }

        private async Task UpdateEmbedAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (!(Arg.Data.CustomId == this.SubmitButton.Id && this.PreviousPageElse != null))
            {
                await U.SaveAsync();
                KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
                ComponentBuilder Com = await this.GetComponentsAsync(U);
                await Arg.UpdateAsync(C =>
                {
                    C.Embed = E.Build();
                    C.Components = Com.Build();
                    C.Content = null;
                });
            }
        }

        #endregion

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = (await this.GetDefaultComponents())
                .WithButton(this.SubmitButton.WithDisabled(this.Listing.CostPerItem <= 0 || !this.Listing.Items.All(I => I.UsersCanSellThis)))
                .WithButton(this.AddOneMoreButton.WithDisabled(U.Settings.Inventory.GetItemsOfDisplayName(this.Listing.Items.First()).Result.Count() <= this.Listing.Items.Count));
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

            this.SubmitButton.OnButtonPush += this.UpdateEmbedAsync;
            this.AddOneMoreButton.OnButtonPush += this.UpdateEmbedAsync;
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
            this.AddOneMoreButton.Dispose();
            this.SubmitButton.Dispose();
        }
    }
}
