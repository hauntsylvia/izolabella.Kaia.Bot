namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    internal class MeView : KaiaPathEmbed
    {
        public MeView(string UserName, KaiaUser User) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Users, UserName)
        {
            this.WithField("highest number counted", $"`{User.Settings.HighestCountEver ?? 0}`");
            this.WithField("total numbers counted", $"`{User.Settings.NumbersCounted ?? 0}`");
            this.WithField($"{Strings.Economy.CurrencyEmote} current {Strings.Economy.CurrencyName}", $"`{User.Settings.Inventory.Petals}`");
        }
    }
}
