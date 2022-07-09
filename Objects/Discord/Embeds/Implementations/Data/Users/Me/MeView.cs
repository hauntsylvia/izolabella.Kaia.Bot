using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Me
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

        protected override async Task ClientRefreshAsync()
        {
            this.WithField("highest number counted", $"`{this.User.Settings.HighestCountEver ?? 0}`");
            this.WithField("total numbers counted", $"`{this.User.Settings.NumbersCounted ?? 0}`");
            this.WithField($"{Strings.Economy.CurrencyEmote} current {Strings.Economy.CurrencyName}", $"`{this.User.Settings.Inventory.Petals}`");
            IEnumerable<UserRelationship> Relationships = await this.User.RelationshipsProcessor.GetRelationshipsAsync();
            if (Relationships.Any())
            {
                this.WithField("relationships", $"in `{Relationships.Count()}` {(Relationships.Count() == 1 ? "relationship" : "relationships")}");
            }
        }
    }
}
