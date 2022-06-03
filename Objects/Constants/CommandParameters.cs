using Discord;
using izolabella.Discord.Objects.Parameters;

namespace Kaia.Bot.Objects.Constants
{
    internal static class CommandParameters
    {
        internal static IzolabellaCommandParameter SomeoneOtherThanMeUser =>
            new("User", "The user I'd like to view.", ApplicationCommandOptionType.User, false);
    }
}
