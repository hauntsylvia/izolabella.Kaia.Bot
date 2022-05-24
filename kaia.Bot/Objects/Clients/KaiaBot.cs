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

namespace Kaia.Bot.Objects.Clients
{
    public class KaiaBot
    {
        public KaiaBot(KaiaParams Parameters)
        {
            this.Parameters = Parameters;
            this.Parameters.CommandHandler.Client.MessageReceived += this.MessageReceived;
            this.MessageReceivers =  InterfaceImplementationController.GetItems<IMessageReceiver>();
        }

        public KaiaParams Parameters { get; }
        public List<IMessageReceiver> MessageReceivers { get; }

        private async Task MessageReceived(SocketMessage Arg)
        {
            if(!Arg.Author.IsBot)
            {
                CCBUser User = new(Arg.Author.Id);
                IMessageReceiver? Receiver = this.MessageReceivers.FirstOrDefault((M) =>
                {
                    return M.CheckMessageValidityAsync(User, Arg).Result;
                });
                if (Receiver != null)
                {
                    try
                    {
                        MessageReceiverResult Result = await Receiver.RunAsync(User, Arg);
                        if(Result.ItemToUse != null && Result.ItemToUse is CountingRefresher CRef)
                        {
                            await Receiver.CallbackAsync(User, Arg, Result);
                            User.Settings.Inventory.Items.Remove(CRef);
                        }
                    }
                    catch (Exception Ex)
                    {
                        await Receiver.OnErrorAsync(Ex);
                    }
                }
                await User.SaveAsync();
            }
        }
    }
}
