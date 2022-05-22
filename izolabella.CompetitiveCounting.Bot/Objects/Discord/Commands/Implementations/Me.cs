using Discord.WebSocket;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Bases;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.CompetitiveCounting.Bot.Objects.Exceptions;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Implementations
{
    public class Me : ICCBCommand
    {
        public string Name => "Me";

        public string Description => "View my statistics.";

        public IzolabellaCommandParameter[] Parameters => Array.Empty<IzolabellaCommandParameter>();

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            await Context.UserContext.RespondAsync(text: "", embed: new MeView(Context.UserContext.User.Username, await CCBUser.GetOrCreateAsync(Context.UserContext.User.Id)).Build());
        }

        public Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }
    }
}
