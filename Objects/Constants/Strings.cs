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
            internal static string BookStore => "Kaia Library";
            internal static string AchievementStore => "Achievements";
            internal static string RateLimitStore => "Rate Limits";
            internal static string SaleListingsStore => "Sale Listings";
        }
        public static class EmbedStrings
        {
            internal static string FooterString => "⊹⊱-☿ Mercury-Izolabella ☿-⊰⊹";
            internal static DateTime DefaultTimestamp => DateTime.UtcNow;
            internal static string Empty => "\u200b";
            internal static string UnknownUser => "unknown user";
            internal static string UnknownGuild => "unknown guild";
            public static class FakePaths
            {
                internal static string NotFound => "not found";
                internal static string Commands => "commands";
                internal static string Users => "users";
                public static string Global => "global";
                internal static string Guilds => "guilds";
                internal static string Settings => "settings";
                internal static string Leaderboards => "leaderboards";
                internal static string StoreOrShop => "store";
                internal static string Library => "library";
                internal static string Inventory => "inventory";
                internal static string Kaia => "kaia";
                internal static string Achievements => "achievements";
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
                internal static string ZeroOrNegativeQuantity => "u don't want to buy anything?";
                internal static string InvalidCurrencyAmount => "u don't have enough to cover this transaction.";
                internal static string NoBookFound => "oh no, there's no matching book.";
            }
            internal static class Counting
            {
                internal static string SameUserTriedCountingTwiceInARow => "sorry! u can't quite do that (someone else has to count next).";
                internal static string UserCountingSaved => "counting refreshed. don't fail next time.";
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

        internal static class ItemStrings
        {
            internal static class Rose
            {
                internal static string Name => "Rose";
                internal static string RoseStab => "ouch! the rose stabbed u . .";
                internal static string RosePretty => "u examine the rose, and find it peaceful.";
            }
            internal static class CountingRefresher
            {
                internal static string Name => "Counting Refresher";
            }
        }
        internal static class Economy
        {
            internal static string CurrencyName => "Petals";
            internal static Emoji CurrencyEmote => Emoji.Parse("💮");
        }
    }
}
