using Discord;
using izolabella.Discord.Objects.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Constants
{
    internal static class Arguments
    {
        internal static IzolabellaCommandParameter SomeoneOtherThanMeUser =>
            new("User", "The user I'd like to view.", ApplicationCommandOptionType.User, false);
    }
}
