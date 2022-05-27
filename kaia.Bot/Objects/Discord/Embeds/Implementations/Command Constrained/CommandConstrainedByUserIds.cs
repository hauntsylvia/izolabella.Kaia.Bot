namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    public class CommandConstrainedByUserIds : KaiaPathEmbed
    {
        public CommandConstrainedByUserIds(string GuildName, string CommandName) : base(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *access*\nYou do not have access to this command.",
            });
        }
    }
}
