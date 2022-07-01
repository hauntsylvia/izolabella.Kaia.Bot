using izolabella.Util.RateLimits.Limiters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    public class InventoryItemView : KaiaItemContentView
    {
        public InventoryItemView(MeInventoryView From, KaiaInventoryItem Item, CommandContext Context, KaiaUser U) : base(From, Context, true)
        {
            this.From = From;
            this.Item = Item;
            this.U = U;
            this.Interact = new(Context, "Interact", Emotes.Counting.InteractItem);
            this.PutUpForSaleButton = new(Context, "Sell", Emotes.Counting.SellItem);

            this.Interact.OnButtonPush += this.InteractAsync;
            this.PutUpForSaleButton.OnButtonPush += this.SellAsync;

            this.Interact.OnButtonPush += this.ButtonPressed;
            this.PutUpForSaleButton.OnButtonPush += this.ButtonPressed;
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
            if (await this.RateLimiter.CheckIfPassesAsync(Arg.User.Id))
            {
                if (Arg.Data.CustomId != this.PutUpForSaleButton.Id)
                {
                    await U.SaveAsync();
                    KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
                    ComponentBuilder Com = await this.GetComponentsAsync(U);
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
            if (this.ListingInteraction == null && this.Item != null)
            {
                this.ListingInteraction = new ItemCreateSaleListing(this, this.Context, new(new() { this.Item }, this.U, this.Item.MarketCost));
            }
            else if(this.ListingInteraction != null)
            {
                this.ListingInteraction.Dispose();
            }

            if(this.ListingInteraction != null)
            {
                await this.ListingInteraction.StartAsync(U);
            }
        }

        private async Task InteractAsync(SocketMessageComponent Arg, KaiaUser U)
        {
            if (this.Item != null && (await U.Settings.Inventory.GetItemOfId(this.Item.Id)) != null)
            {
                await this.Context.UserContext.FollowupAsync(embed: new InteractWithItemEmbed(this.Item, U).Build());
                this.Item = await U.Settings.Inventory.GetItemOfDisplayName(this.Item);
            }
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            KaiaPathEmbedRefreshable V = this.Item == null ? new SingleItemNotFound() : new InventoryItemViewRaw(this.Item);
            await V.RefreshAsync();
            return V;
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            ComponentBuilder CB = await this.GetDefaultComponents();
            CB.WithButton(this.Interact.WithDisabled(!await U.Settings.Inventory.ItemOfDisplayNameExists(this.Item) || (!this.Item?.CanInteractWithDirectly ?? true)));
            if (this.Item != null && this.Item.UsersCanSellThis)
            {
                CB.WithButton(this.PutUpForSaleButton.WithDisabled(!await U.Settings.Inventory.ItemOfDisplayNameExists(this.Item) || !this.Item.UsersCanSellThis));
            }
            return CB;
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

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Interact.Dispose();
            this.PutUpForSaleButton.Dispose();
        }
    }
}
