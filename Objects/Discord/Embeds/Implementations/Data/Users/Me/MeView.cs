using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeViews
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
            WithField("highest number counted", $"`{User.Settings.HighestCountEver ?? 0}`");
            WithField("total numbers counted", $"`{User.Settings.NumbersCounted ?? 0}`");
            WithField($"{Strings.Economy.CurrencyEmote} current {Strings.Economy.CurrencyName}", $"`{User.Settings.Inventory.Petals}`");
            IEnumerable<UserRelationship> Relationships = await User.RelationshipsProcessor.GetRelationshipsAsync();
            if (Relationships.Any())
            {
                WithField("relationships", $"in `{Relationships.Count()}` {(Relationships.Count() == 1 ? "relationship" : "relationships")}");
            }
        }
    }
}
