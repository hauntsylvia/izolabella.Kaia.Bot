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

namespace Kaia.Bot.Objects.Clients
{
    public class KaiaBot
    {
        public KaiaBot(KaiaParams Parameters)
        {
            this.Parameters = Parameters;
            this.Parameters.CommandHandler.Client.MessageReceived += this.MessageReceived;
            this.MessageReceivers = GetMessageReceivers();
        }

        public KaiaParams Parameters { get; }
        public List<IMessageReceiver> MessageReceivers { get; }

        private async Task MessageReceived(SocketMessage Arg)
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
                    await Receiver.RunAsync(User, Arg);
                }
                catch (Exception Ex)
                {
                    await Receiver.OnErrorAsync(Ex);
                }
            }
        }

        private static List<IMessageReceiver> GetMessageReceivers()
        {
            List<IMessageReceiver> R = new();
            foreach (Type T in Assembly.GetCallingAssembly().GetTypes())
            {
                if (typeof(IMessageReceiver).IsAssignableFrom(T) && !T.IsInterface)
                {
                    object? O = Activator.CreateInstance(T);
                    if (O != null && O is IMessageReceiver M)
                    {
                        R.Add(M);
                    }
                }
            }
            return R;
        }
    }
}
