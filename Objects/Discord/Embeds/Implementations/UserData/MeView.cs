using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    internal class MeView : KaiaPathEmbed
    {
        public MeView(string UserName, KaiaUser User) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Users, UserName)
        {
            this.WriteField("highest number counted", $"`{User.Settings.HighestCountEver ?? 0}`");
            this.WriteField("total numbers counted", $"`{User.Settings.NumbersCounted ?? 0}`");
            this.WriteField($"{Strings.Economy.CurrencyEmote} current {Strings.Economy.CurrencyName}", $"`{User.Settings.Inventory.Petals}`");
        }
    }
}
