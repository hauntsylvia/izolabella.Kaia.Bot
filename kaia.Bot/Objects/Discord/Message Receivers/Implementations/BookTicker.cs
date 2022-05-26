using Discord;
using Discord.WebSocket;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Events.Interfaces;
using Kaia.Bot.Objects.Exceptions;
using Kaia.Bot.Objects.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaia.Bot.Objects.CCB_Structures.Users;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Properties;
using Kaia.Bot.Objects.Discord.Message_Receivers.Results;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Implementations;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.Discord.Message_Receivers.Implementations
{
    internal class BookTicker : IMessageReceiver
    {
        public string Name => "Book Ticker";

        public Task<bool> CheckMessageValidityAsync(CCBUser Author, SocketMessage Message)
        {
            return Task.FromResult(true);
        }

        public async Task<MessageReceiverResult> RunAsync(CCBUser Author, SocketMessage? Message)
        {
            MessageReceiverResult Result = new();
            //List<KaiaBook> UserOwnedBooks = await Author.Settings.LibraryProcessor.GetUserBooksAsync();
            //double TotalToPay = 0.0;
            //foreach(KaiaBook Book in UserOwnedBooks)
            //{
            //    double CyclesMissed = ((DateTime.UtcNow - Author.Settings.Inventory.LastBookUpdate) / TimeSpans.BookTickRate);
            //    TotalToPay += Book.CurrentEarning * CyclesMissed;
            //}
            //Author.Settings.Inventory.LastBookUpdate = DateTime.UtcNow;
            //Author.Settings.Inventory.Petals += TotalToPay;
            return Result;
        }

        public Task CallbackAsync(CCBUser Author, SocketMessage Message, MessageReceiverResult CausedCallback)
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }
    }
}
