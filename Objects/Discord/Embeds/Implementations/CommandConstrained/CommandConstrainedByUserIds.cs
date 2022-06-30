﻿namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    public class CommandConstrainedByUserIds : KaiaPathEmbedRefreshable
    {
        public CommandConstrainedByUserIds(string GuildName, string CommandName) : base(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            this.GuildName = GuildName;
            this.CommandName = CommandName;
        }

        public string GuildName { get; }

        public string CommandName { get; }

        public override Task ClientRefreshAsync()
        {
            this.WithField(Strings.EmbedStrings.Empty, $"// *access*\nYou do not have access to this command.");
            return Task.CompletedTask;
        }
    }
}
