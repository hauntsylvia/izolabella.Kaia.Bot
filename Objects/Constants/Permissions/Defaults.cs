using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Constants.Permissions
{
    internal class DefaultPerms
    {
        internal static GuildPermission[] Default => new GuildPermission[]
        {
            GuildPermission.ViewChannel,
            GuildPermission.SendMessages,
            GuildPermission.SendMessagesInThreads,
            GuildPermission.UseExternalEmojis,
            GuildPermission.AddReactions,
            GuildPermission.UseApplicationCommands
        };
    }
}
