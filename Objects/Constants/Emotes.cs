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
            internal static Emoji Cancel => Emoji.Parse("◀️");
            internal static Emoji Book => Emoji.Parse("📖");
            internal static Emoji Inventory => Emoji.Parse("🎀");
            internal static Emoji Location => Emoji.Parse("☁️");
            internal static Emoji BuyItem => Emoji.Parse("🛒");
            internal static Emoji InteractItem => Emoji.Parse("🖋️");
            internal static Emoji SellItem => Emoji.Parse("🧧");
            internal static Emoji Add => Emoji.Parse("➕");
            internal static Emoji Sub => Emoji.Parse("➖");
            internal static Emoji Explore => Emoji.Parse("☁️");
            internal static KaiaEmote Blessings => new("❤️‍🩹");
            internal static KaiaEmote Curses => new("☠️");
        }
        internal static class Items
        {
            internal static KaiaEmote CountingRefresher => new("🔄");
            internal static KaiaEmote Rose => new("🌹");
            internal static KaiaEmote Notebook => new("📒");
            internal static KaiaEmote DeadFinger => new("☠️");
            internal static KaiaEmote NutAndBolt => new("🔩");
            internal static KaiaEmote Cigarette => new("🚬");
            internal static KaiaEmote Candle => new("🕯️");
            internal static Emoji NoEmote => new("❔");
        }
        internal static class Embeds
        {
            internal static Emoji Back => Emoji.Parse("◀️");
            internal static Emoji Forward => Emoji.Parse("▶️");
            internal static Emoji Reverse => Emoji.Parse("⏪");
        }
        internal static class Achievements
        {
            internal static KaiaEmote Counting => new("🔢");
        }
        internal static class Customs
        {
            internal static Emote KaiaDot => Emote.Parse("<:kaiadot:993506891604967504>");
        }
    }
}
