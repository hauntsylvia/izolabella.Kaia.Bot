using Kaia.Bot.Objects.Client_Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.WebSocket;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Events.Interfaces;
using System.Reflection;
using Kaia.Bot.Objects.CCB_Controllers;
using Kaia.Bot.Objects.Discord.Message_Receivers.Results;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Implementations;
using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using Kaia.Bot.Objects.Discord.Message_Receivers.Implementations;

namespace Kaia.Bot.Objects.Clients
{
    public class KaiaBot
    {
        public KaiaBot(KaiaParams Parameters)
        {
            this.Parameters = Parameters;
            this.Parameters.CommandHandler.CommandInvoked += this.AfterCommandExecutedAsync;
            this.Parameters.CommandHandler.Client.MessageReceived += this.MessageReceivedAsync;
            this.MessageReceivers = InterfaceImplementationController.GetItems<IMessageReceiver>();
        }
        public KaiaParams Parameters { get; }

        public List<IMessageReceiver> MessageReceivers { get; }

        private async Task AfterCommandExecutedAsync(izolabella.Discord.Objects.Arguments.CommandContext Context, izolabella.Discord.Objects.Parameters.IzolabellaCommandArgument[] Arguments, izolabella.Discord.Objects.Interfaces.IIzolabellaCommand CommandInvoked)
        {
            await new CCBUser(Context.UserContext.User.Id).SaveAsync();
            if(Context.UserContext.User is SocketGuildUser SU)
            {
                await new CCBGuild(SU.Guild.Id).SaveAsync();
            }
            Console.WriteLine("saved data");
        }

        private async Task MessageReceivedAsync(SocketMessage Arg)
        {
            if(!Arg.Author.IsBot)
            {
                CCBUser User = new(Arg.Author.Id);
                IEnumerable<IMessageReceiver> Receivers = this.MessageReceivers.Where((M) =>
                {
                    return M.CheckMessageValidityAsync(User, Arg).Result;
                });
                foreach(IMessageReceiver Receiver in Receivers)
                {
                    if (Receiver != null)
                    {
                        try
                        {
                            MessageReceiverResult Result = await Receiver.RunAsync(User, Arg);
                            if (Result.ItemToUse != null && Result.ItemToUse is CountingRefresher CRef)
                            {
                                await Receiver.CallbackAsync(User, Arg, Result);
                                User.Settings.Inventory.Items.Remove(CRef);
                            }
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine($"message receiver {Receiver.Name} error => {Ex.Message}");
                            await Receiver.OnErrorAsync(Ex);
                        }
                    }
                }
                await User.SaveAsync();
            }
        }
    }
}
