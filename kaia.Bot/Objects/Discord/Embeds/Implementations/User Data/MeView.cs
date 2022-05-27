using Kaia.Bot.Objects.KaiaStructures;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    internal class MeView : KaiaPathEmbed
    {
        public MeView(string UserName, KaiaUser User) : base(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Users, UserName)
        {
            this.WriteField("highest number counted", $"`{User.Settings.HighestCountEver ?? 0}`");
            this.WriteField("total numbers counted", $"`{User.Settings.NumbersCounted ?? 0}`");
            this.WriteField($"{Strings.Economy.CurrencyEmote} current {Strings.Economy.CurrencyName}", $"`{User.Settings.Inventory.Petals}`");
        }
    }
}
