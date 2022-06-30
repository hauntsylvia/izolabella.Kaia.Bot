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

        public override Task RefreshAsync()
        {
            string MissingStr = Strings.EmbedStrings.Empty;
            GuildPermission[] MissingGP = this.Required.Where(P => !this.UserHas.Has(P)).ToArray();
            foreach (GuildPermission P in MissingGP)
            {
                MissingStr += $"{(MissingStr != Strings.EmbedStrings.Empty ? ", " : "")}{Regex.Replace(P.ToString(), "([A-Z])", " $1").ToLower(CultureInfo.InvariantCulture)}";
            }
            this.Fields.Add(new()
            {
                Name = $"{Strings.EmbedStrings.Empty}",
                Value = $"// *missing permissions*\n {MissingStr}",
            });
            return Task.CompletedTask;
        }
    }
}
