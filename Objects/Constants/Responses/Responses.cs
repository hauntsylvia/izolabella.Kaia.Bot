using Kaia.Bot.Objects.Constants.Embeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Constants.Responses
{
    internal class Responses
    {
        internal static async Task PipeErrors(CommandContext Context, KaiaPathEmbed E)
        {
            if (Context.UserContext.HasResponded)
            {
                await Context.UserContext.FollowupAsync(embed: E.Build(), ephemeral: true);
            }
            else
            {
                await Context.UserContext.RespondAsync(embed: E.Build(), ephemeral: true);
            }
        }
    }
}
