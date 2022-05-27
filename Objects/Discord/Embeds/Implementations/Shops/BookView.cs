using Discord;
using izolabella.Discord.Objects.Arguments;
using Kaia.Bot.Objects.Discord.Commands.Implementations;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;
using Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary;
using Kaia.Bot.Objects.KaiaStructures.Users;
using Kaia.Bot.Objects.Util;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class BookView : IKaiaItemContentView
    {
        public BookView(CommandContext Context, string BookId, IEmote ReadPageEmote)
        {
            this.ReadNextPageId = $"readnextpage-lbv-{IdGenerator.CreateNewId()}";
            this.BackId = $"goback-{IdGenerator.CreateNewId()}";
            this.Context = Context;
            this.BookId = BookId;
            this.ReadPageEmote = ReadPageEmote;
        }

        public string ReadNextPageId { get; }
        public string BackId { get; }
        public CommandContext Context { get; }
        public string BookId { get; }
        public IEmote? GoBackView => Emotes.Embeds.Reverse;
        public IEmote ReadPageEmote { get; }

        public event IKaiaItemContentView.GoBackHandler? BackRequested;
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Context.Reference.Client.ButtonExecuted -= this.ButtonExecutedAsync;
        }

        private async Task<KaiaBook?> GetUserBookAsync(KaiaUser U)
        {
            return (await U.Settings.LibraryProcessor.GetUserBooksAsync()).FirstOrDefault(B => B.BookId == this.BookId) ?? KaiaLibrary.GetActiveBookById(this.BookId) ?? null;
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            KaiaBook? Book = await this.GetUserBookAsync(U);
            ComponentBuilder CB = new ComponentBuilder().WithButton("Read", this.ReadNextPageId, ButtonStyle.Secondary, this.ReadPageEmote, disabled: Book != null && Book.CurrentPageIndex >= Book.Pages);
            if(this.GoBackView != null)
            {
                CB.WithButton("Back", this.BackId, ButtonStyle.Secondary, disabled: false, emote: this.GoBackView);
            }
            return CB;
        }

        public async Task<KaiaPathEmbed> GetEmbedAsync(KaiaUser U)
        {
            KaiaBook? Book = await this.GetUserBookAsync(U);
            KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Library, Book?.Title ?? Strings.EmbedStrings.FakePaths.NotFound);
            if (Book != null)
            {
                Embed.WriteField($"author", $"`{Book.Author}`");
                Embed.WriteField($"title", $"`{Book.Title}`");
                Embed.WriteField($"current page", $"`{Book.CurrentPageIndex}` / `{Book.Pages}`");
                Embed.WriteField($"{Strings.Economy.CurrencyEmote} current earnings", $"{Strings.Economy.CurrencyEmote} `{Book.CurrentEarning}` / `{TimeSpans.BookTickRate.TotalMinutes}` min.");
                if (Book.CurrentPageIndex <= Book.Pages)
                {
                    Embed.WriteField($"{Strings.Economy.CurrencyEmote} cost to read next page", $"{Strings.Economy.CurrencyEmote} `{Book.NextPageTurnCost}`");
                }
            }
            return Embed;
        }

        public async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbed E = await this.GetEmbedAsync(U);
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
            if ((Component.Data.CustomId == this.ReadNextPageId || Component.Data.CustomId == this.BackId) && Component.User.Id == this.Context.UserContext.User.Id)
            {
                if(Component.Data.CustomId != this.BackId)
                {
                    KaiaUser U = new(Component.User.Id);
                    KaiaBook? Book = await this.GetUserBookAsync(U);
                    if (Book != null && U.Settings.Inventory.Petals >= Book.NextPageTurnCost)
                    {
                        if (!await U.Settings.LibraryProcessor.UserHasBookOfIdAsync(this.BookId) && KaiaLibrary.GetActiveBookById(this.BookId) is KaiaBook KBook)
                        {
                            await U.Settings.LibraryProcessor.AddBookAsync(KBook);
                        }
                        U.Settings.Inventory.Petals -= Book.NextPageTurnCost;
                        await U.Settings.LibraryProcessor.IncrementBookAsync(Book.BookId);
                        await U.SaveAsync();
                    }
                    KaiaPathEmbed E = await this.GetEmbedAsync(U);
                    ComponentBuilder Com = await this.GetComponentsAsync(U);
                    await Component.UpdateAsync(C =>
                    {
                        C.Embed = E.Build();
                        C.Components = Com.Build();
                    });
                }
                else
                {
                    this.BackRequested?.Invoke(Component);
                }
            }
        }
    }
}
