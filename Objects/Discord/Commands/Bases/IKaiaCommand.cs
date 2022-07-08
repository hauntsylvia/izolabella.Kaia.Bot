using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Commands.Bases
{
    public interface IKaiaCommand
    {
        /// <summary>
        /// DO NOT CHANGE after first compilation with the command.
        /// </summary>
        public string ForeverId { get; }

        public string Name { get; }

        public List<GuildPermission> RequiredPermissions { get; }
    }
}
