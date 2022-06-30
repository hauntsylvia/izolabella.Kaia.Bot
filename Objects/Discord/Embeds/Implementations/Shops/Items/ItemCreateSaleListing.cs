using izolabella.Util;
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
            this.Id = IdGenerator.CreateNewId();
            this.SubmitButtonId = $"{this.Id}-{IdGenerator.CreateNewId()}";
            this.AddMoreButtonId = $"{this.Id}-{IdGenerator.CreateNewId()}";
            PreviousPage.ListingInteraction = this;
            this.PreviousPage = PreviousPage;
        }

        public SaleListing Listing { get; }

        public ulong Id { get; }

        public string SubmitButtonId { get; }

        public Emoji SubmitButtonEmote { get; } = Emotes.Counting.SellItem;

        public string AddMoreButtonId { get; }

        public Emoji AddButtonEmote { get; } = Emotes.Counting.Add;
        public ItemView PreviousPage { get; }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = (await this.GetDefaultComponents())
                .WithButton("Submit",
                           this.SubmitButtonId,
                           ButtonStyle.Secondary,
                           this.SubmitButtonEmote,
                           disabled: this.Listing.CostPerItem <= 0 || !this.Listing.Items.All(I => I.UsersCanSellThis))
                .WithButton("Add",
                           this.AddMoreButtonId,
                           ButtonStyle.Secondary,
                           this.AddButtonEmote,
                           disabled: U.Settings.Inventory.GetItemsOfDisplayName(this.Listing.Items.First()).Result.Count() <= this.Listing.Items.Count);
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
            this.Context.Reference.Client.ButtonExecuted += this.ButtonExecutedAsync;
        }

        private async Task ButtonExecutedAsync(SocketMessageComponent Arg)
        {
            if (Arg.IsValidToken
                && (Arg.Data.CustomId == this.SubmitButtonId
                    || Arg.Data.CustomId == this.AddMoreButtonId)
                && Arg.User.Id == this.Context.UserContext.User.Id)
            {
                KaiaUser U = new(Arg.User.Id);
                IEnumerable<KaiaInventoryItem> ItemsMinusExistingIdsInThisListing = U.Settings.Inventory.Items.Where(I => I.UsersCanSellThis && !this.Listing.Items.Any(ListingItem => ListingItem.Id == I.Id));
                KaiaInventoryItem? RelevantItem = await U.Settings.Inventory.GetItemOfId(ItemsMinusExistingIdsInThisListing.FirstOrDefault()?.Id ?? 0);
                if (Arg.Data.CustomId == this.SubmitButtonId)
                {
                    await Arg.DeferAsync();
                    await this.Listing.StartSellingAsync();
                    this.Dispose();
                    this.PreviousPage.Dispose();
                    if(this.PreviousPage.From != null)
                    {
                        this.PreviousPage.From.Dispose();
                        await new ItemsPaginated(this.Context, this.PreviousPage.From.FilterBy, this.PreviousPage.From.IncludeUserListings, this.PreviousPage.From.ChunkSize).StartAsync();
                    }
                    else
                    {
                        await new ItemsPaginated(this.Context).StartAsync();
                    }
                }
                else if (Arg.Data.CustomId == this.AddMoreButtonId && RelevantItem != null && RelevantItem.UsersCanSellThis)
                {
                    this.Listing.Items.Add(RelevantItem);
                }
                if(!(Arg.Data.CustomId == this.SubmitButtonId && this.PreviousPageElse != null))
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
        }

        public override Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            ItemCreateSaleListingRaw Em = new(this.Listing);
            return Task.FromResult<KaiaPathEmbedRefreshable>(Em);
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
