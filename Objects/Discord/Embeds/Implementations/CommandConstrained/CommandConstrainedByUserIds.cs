using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    public class CommandConstrainedByUserIds(string GuildName, string CommandName) : KaiaPathEmbedRefreshable(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
    {
        public string GuildName { get; } = GuildName;

        public string CommandName { get; } = CommandName;

        protected override Task ClientRefreshAsync()
        {
            this.WithField(Strings.EmbedStrings.Empty, $"// *access*\nYou do not have access to this command.");
            return Task.CompletedTask;
        }
    }
}