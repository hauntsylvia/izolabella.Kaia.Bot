using izolabella.CompetitiveCounting.Bot.Objects.Client_Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.WebSocket;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Events.Interfaces;
using System.Reflection;

namespace izolabella.CompetitiveCounting.Bot.Objects.Clients
{
    public class CompetitiveCountingBot
    {
        public CompetitiveCountingBot(ClientParameters Parameters)
        {
            this.Parameters = Parameters;
            this.Parameters.CommandHandler.Client.MessageReceived += this.MessageReceived;
            this.MessageReceivers = GetMessageReceivers();
        }

        public ClientParameters Parameters { get; }
        public List<IMessageReceiver> MessageReceivers { get; }

        private async Task MessageReceived(SocketMessage Arg)
        {
            CCBUser User = await CCBUser.GetOrCreateAsync(Arg.Author.Id);
            IMessageReceiver? Receiver = this.MessageReceivers.FirstOrDefault((M) =>
            {
                return M.CheckMessageValidityAsync(User, Arg).Result;
            });
            if(Receiver != null)
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
