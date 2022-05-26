using Discord;
using Discord.WebSocket;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Bases;
using Kaia.Bot.Objects.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class ItemView : ICCBItemContentView
    {
        public ItemView(CommandContext Context, ICCBInventoryItem Item, IEmote BuyItemEmote)
        {
            this.Context = Context;
            this.Item = Item;
            this.InteractionEmote = BuyItemEmote;
        }

        public CommandContext Context { get; }
        public ICCBInventoryItem Item { get; }
        public IEmote InteractionEmote { get; }
        public ulong RId { get; } = IdGenerator.CreateNewId();
        public string CustomId => $"{this.Item.DisplayName}-{this.RId}";

        public Task<ComponentBuilder> GetComponentsAsync(CCBUser U)
        {
            return Task.FromResult(new ComponentBuilder().
                WithButton("Buy",
                           this.CustomId,
                           ButtonStyle.Secondary,
                           this.InteractionEmote,
                           disabled: U.Settings.Inventory.Petals < this.Item.Cost));
        }

        public Task<CCBPathEmbed> GetEmbedAsync(CCBUser U)
        {
            CCBPathEmbed Em = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.StoreOrShop, this.Item.DisplayName);

            Em.WriteField($"[{Strings.Economy.CurrencyEmote} `{this.Item.Cost}`] {this.Item.DisplayName}  {this.Item.DisplayEmote}", this.Item.Description);
            Em.WriteField("your balance", $"{Strings.Economy.CurrencyEmote} `{U.Settings.Inventory.Petals}`");
            Em.WriteField($"number of {this.Item.DisplayName}s owned", $"`{U.Settings.Inventory.Items.Count(I => I.DisplayName == this.Item.DisplayName)}`");
            return Task.FromResult(Em);
        }

        public async Task StartAsync(CCBUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            CCBPathEmbed E = await this.GetEmbedAsync(U);
            ComponentBuilder Com = await this.GetComponentsAsync(U);
            await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
            this.Context.Reference.Client.ButtonExecuted += this.ButtonExecutedAsync;
        }

        private async Task ButtonExecutedAsync(SocketMessageComponent Arg)
        {
            if (Arg.Data.CustomId == this.CustomId && Arg.User.Id == this.Context.UserContext.User.Id)
            {
                CCBUser U = new(Arg.User.Id);
                if(U.Settings.Inventory.Petals >= this.Item.Cost)
                {
                    U.Settings.Inventory.Petals -= this.Item.Cost;
                    U.Settings.Inventory.Items.Add(this.Item);
                    await this.Item.UserBoughtAsync(this.Context, U);
                }
                await U.SaveAsync();
                CCBPathEmbed E = await this.GetEmbedAsync(U);
                ComponentBuilder Com = await this.GetComponentsAsync(U);
                await Arg.UpdateAsync(C =>
                {
                    C.Embed = E.Build();
                    C.Components = Com.Build();
                });
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.Client.ButtonExecuted -= this.ButtonExecutedAsync;
        }
    }
}
