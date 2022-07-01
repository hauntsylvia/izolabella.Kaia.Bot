namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    public class MeView : KaiaPathEmbedRefreshable
    {
        public MeView(string UserName, KaiaUser User) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Users, UserName)
        {
            this.UserName = UserName;
            this.User = User;
        }

        public string UserName { get; }

        public KaiaUser User { get; }

        protected override Task ClientRefreshAsync()
        {
            this.WithField("highest number counted", $"`{this.User.Settings.HighestCountEver ?? 0}`");
            this.WithField("total numbers counted", $"`{this.User.Settings.NumbersCounted ?? 0}`");
            this.WithField($"{Strings.Economy.CurrencyEmote} current {Strings.Economy.CurrencyName}", $"`{this.User.Settings.Inventory.Petals}`");
            return Task.CompletedTask;
        }
    }
}
