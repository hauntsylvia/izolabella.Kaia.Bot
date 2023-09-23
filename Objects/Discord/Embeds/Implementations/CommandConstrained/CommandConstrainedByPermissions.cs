using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using System.Text.RegularExpressions;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    public class CommandConstrainedByPermissions(string GuildName, string CommandName, GuildPermissions UserHas, GuildPermission[] Required) : KaiaPathEmbedRefreshable(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
    {
        public string GuildName { get; } = GuildName;

        public string CommandName { get; } = CommandName;

        public GuildPermissions UserHas { get; } = UserHas;

        public GuildPermission[] Required { get; } = Required;

        protected override Task ClientRefreshAsync()
        {
            List<string> Display = new();
            GuildPermission[] MissingGP = this.Required.Where(P => !this.UserHas.Has(P)).ToArray();
            foreach (GuildPermission P in MissingGP)
            {
                Display.Add($"{Emotes.Customs.KaiaDot}{Regex.Replace(P.ToString(), "([A-Z])", " $1").ToLower(CultureInfo.InvariantCulture)}");
            }
            this.WithListWrittenToField($"missing permissions", Display, "\n");
            return Task.CompletedTask;
        }
    }
}