using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures.Derivations;
using izolabella.CompetitiveCounting.Bot.Objects.Exceptions;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Bases
{
    public interface ICCBCommand : IIzolabellaCommand
    {
        /// <summary>
        /// DO NOT CHANGE after first compilation with the command.
        /// </summary>
        public string ForeverId { get; }
    }
}
