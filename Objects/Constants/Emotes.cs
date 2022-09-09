using izolabella.Kaia.Bot.Objects.Util;

namespace izolabella.Kaia.Bot.Objects.Constants;

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
        internal static KaiaEmote Heart => new("💞");
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
        internal static KaiaEmote KaiaDot => new("<:kaiadot:993506891604967504>");
        internal static KaiaEmote KaiaWelcome => new("<a:welcome:993566617151737906>");

        internal static class Numbers
        {
            internal static KaiaEmote Kaia1 => new("<:number1:993544420081291354>");
            internal static KaiaEmote Kaia2 => new("<:number2:993544418088976424>");
            internal static KaiaEmote Kaia3 => new("<:number3:993544418994954371>");
            internal static KaiaEmote Kaia4 => new("<:number4:993544414897115197>");
            internal static KaiaEmote Kaia5 => new("<:number5:993544415786303489>");
            internal static KaiaEmote Kaia6 => new("<:number6:993544416918777936>");
            internal static KaiaEmote Kaia7 => new("<:number7:993544413064212550>");
            internal static KaiaEmote Kaia8 => new("<:number8:993544414054055996>");
            internal static KaiaEmote Kaia9 => new("<:number9:993544411596206151>");
        }
    }
}
