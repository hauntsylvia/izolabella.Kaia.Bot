using System.Text.RegularExpressions;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained
{
    public class CommandConstrainedByPermissions : KaiaPathEmbedRefreshable
    {
        public CommandConstrainedByPermissions(string GuildName, string CommandName, GuildPermissions UserHas, GuildPermission[] Required) : base(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            this.GuildName = GuildName;
            this.CommandName = CommandName;
            this.UserHas = UserHas;
            this.Required = Required;
        }

        public string GuildName { get; }

        public string CommandName { get; }

        public GuildPermissions UserHas { get; }

        public GuildPermission[] Required { get; }

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
