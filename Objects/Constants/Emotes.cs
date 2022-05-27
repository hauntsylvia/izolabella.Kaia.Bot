using Discord;

namespace Kaia.Bot.Objects.Constants
{
    internal static class Emotes
    {
        internal static class Counting
        {
            internal static Emoji Check => Emoji.Parse("☑️");
            internal static Emoji CheckRare => Emoji.Parse("💮");
            internal static Emoji ThumbDown => Emoji.Parse("👎");
            internal static Emoji Invalid => Emoji.Parse("❌");
            internal static Emoji Book => Emoji.Parse("📖");
            internal static Emoji Inventory => Emoji.Parse("🎀");
            internal static Emoji BuyItem => Emoji.Parse("🛒");
            internal static Emoji InteractItem => Emoji.Parse("🖋️");
        }
        internal static class Items
        {
            internal static Emoji CountingRefresher => Emoji.Parse("🔄");
        }
        internal static class Embeds
        {
            internal static Emoji Back => Emoji.Parse("◀️");
            internal static Emoji Forward => Emoji.Parse("▶️");
        }
    }
}
