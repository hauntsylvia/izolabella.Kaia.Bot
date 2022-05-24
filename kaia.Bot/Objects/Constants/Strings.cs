using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Constants
{
    public static class Strings
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
        public static class EmbedStrings
        {
            internal static string FooterString => "⊹⊱-☿ Mercury-Izolabella ☿-⊰⊹";
            internal static DateTime DefaultTimestamp => DateTime.UtcNow;
            internal static string Empty => "\u200b";
            internal static string UnknownUser => "unknown user";
            public static string PathIfNoGuild => "global";
            internal static class FakePaths
            {
                internal static string Commands => "commands";
                internal static string Users => "users";
                internal static string Settings => "settings";
                internal static string Leaderboards => "leaderboards";
                internal static string StoreOrShop => "store";
            }
        }
        internal static class Responses
        {
            internal static class Commands
            {
                internal static string GuildSetSaved => "guild settings saved!";
                internal static string GuildSetSaveFail => "guild settings could not be saved.";
                internal static string InvalidLeaderboardOption => "that's an invalid leaderboard option!";
                internal static string NoInventoryItemWithThatNameFound => "there r no items matching that name . .";
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
        internal static class Economy
        {
            internal static string CurrencyName => "Petals";
            internal static Emoji CurrencyEmote => Emoji.Parse("💮");
        }
    }
}
