using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Implementations;
using Kaia.Bot.Objects.CCB_Structures.Books.Properties;
using Kaia.Bot.Objects.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class BookView : ICCBItemContentView
    {
        public BookView(CommandContext Context, string BookId, IEmote ReadPageEmote)
        {
            this.ReadNextPageId = $"readnextpage-lbv-{IdGenerator.CreateNewId()}";
            this.Context = Context;
            this.BookId = BookId;
            this.BuyItemEmote = ReadPageEmote;
        }

        public string ReadNextPageId { get; }
        public CommandContext Context { get; }
        public string BookId { get; }
        public IEmote BuyItemEmote { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.Client.ButtonExecuted -= this.ButtonExecutedAsync;
        }

        private async Task<KaiaBook> GetUserBookAsync(CCBUser U)
        {
            return (await U.Settings.LibraryProcessor.GetUserBooksAsync()).FirstOrDefault(B => B.BookId == this.BookId) is KaiaBook KBookA
                ? KBookA ?? throw new NullReferenceException()
                : KaiaLibrary.GetActiveBookById(this.BookId) ?? throw new NullReferenceException();
            throw new NullReferenceException();
        }

        public async Task<ComponentBuilder> GetComponentsAsync(CCBUser U)
        {
            KaiaBook Book = await this.GetUserBookAsync(U);
            return new ComponentBuilder().WithButton("Read", this.ReadNextPageId, ButtonStyle.Secondary, this.BuyItemEmote, disabled: Book.CurrentPageIndex >= Book.Pages);
        }

        public async Task<CCBPathEmbed> GetEmbedAsync(CCBUser U)
        {
            KaiaBook Book = await this.GetUserBookAsync(U);
            CCBPathEmbed Embed = new(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Library, Book.Title);
            if (Book != null)
            {
                Embed.WriteField($"author", $"`{Book.Author}`");
                Embed.WriteField($"title", $"`{Book.Title}`");
                Embed.WriteField($"current page", $"`{Book.CurrentPageIndex}` / `{Book.Pages}`");
                Embed.WriteField($"{Strings.Economy.CurrencyEmote} current earnings", $"{Strings.Economy.CurrencyEmote} `{Book.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                if(Book.CurrentPageIndex <= Book.Pages)
                {
                    Embed.WriteField($"{Strings.Economy.CurrencyEmote} cost to read next page", $"{Strings.Economy.CurrencyEmote} `{Book.NextPageTurnCost}`");
                }
            }
            return Embed;
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

        private async Task ButtonExecutedAsync(global::Discord.WebSocket.SocketMessageComponent Component)
        {
            if(Component.Data.CustomId == this.ReadNextPageId && Component.User.Id == this.Context.UserContext.User.Id)
            {
                CCBUser U = new(Component.User.Id);
                KaiaBook Book = await this.GetUserBookAsync(U);
                if (U.Settings.Inventory.Petals >= Book.NextPageTurnCost)
                {
                    if(!(await U.Settings.LibraryProcessor.UserHasBookOfIdAsync(this.BookId)) && KaiaLibrary.GetActiveBookById(this.BookId) is KaiaBook KBook)
                    {
                        await U.Settings.LibraryProcessor.AddBookAsync(KBook);
                    }
                    U.Settings.Inventory.Petals -= Book.NextPageTurnCost;
                    await U.Settings.LibraryProcessor.IncrementBookAsync(Book.BookId);
                    await U.SaveAsync();
                }
                CCBPathEmbed E = await this.GetEmbedAsync(U);
                ComponentBuilder Com = await this.GetComponentsAsync(U);
                await Component.UpdateAsync(C =>
                {
                    C.Embed = E.Build();
                    C.Components = Com.Build();
                });
            }
        }
    }
}
