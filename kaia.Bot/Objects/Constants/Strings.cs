using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Constants
{
    internal static class Strings
    {
        internal static class App
        {
            internal static string Name => "CompetitiveCounting.Bot";
        }
        internal static class DataStoreNames
        {
            internal static string UserStore => "CCBUsers";
            internal static string GuildStore => "CCBGuilds";
        }
        internal static class EmbedStrings
        {
            internal static string FooterString => "⊹⊱-☿ Mercury-Izolabella ☿-⊰⊹";
            internal static DateTime DefaultTimestamp => DateTime.UtcNow;
            internal static string Empty => "\u200b";
        }
        internal static class Responses
        {
            internal static class Commands
            {
                internal static string GuildSetSaved => "guild settings saved!";
                internal static string GuildSetSaveFail => "guild settings could not be saved.";
            }
            internal static string SameUserTriedCountingTwiceInARow => "sorry! u can't quite do that (someone else has to count next).";
            private static string[] UserFailedInCounting => new[] 
            { 
                "oh my . . how unfortunate. time to start over!",
                "one day.",
                "yikes.",
                "maybe u'll get it next time?",
                "i feel sorta bad for this one.",
            };
            internal static string GetRandomCountingFailText()
            {
                return UserFailedInCounting[new Random().Next(0, UserFailedInCounting.Length)];
            }
        }
    }
}
