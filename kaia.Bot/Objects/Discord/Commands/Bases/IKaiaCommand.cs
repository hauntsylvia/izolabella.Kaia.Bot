﻿using izolabella.Discord.Objects.Interfaces;

namespace Kaia.Bot.Objects.Discord.Commands.Bases
{
    public interface IKaiaCommand : IIzolabellaCommand
    {
        /// <summary>
        /// DO NOT CHANGE after first compilation with the command.
        /// </summary>
        public string ForeverId { get; }
    }
}