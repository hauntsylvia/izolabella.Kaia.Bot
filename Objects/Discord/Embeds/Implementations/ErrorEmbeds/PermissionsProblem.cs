using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using System.Text.RegularExpressions;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds
{
    public class PermissionsProblem : KaiaPathEmbed
    {
        public PermissionsProblem(string GuildName, string CommandName, GuildPermissions KaiaHas, GuildPermission[] Required) : base(GuildName, Strings.EmbedStrings.FakePaths.Commands, CommandName)
        {
            this.GuildName = GuildName;
            this.CommandName = CommandName;
            this.KaiaHas = KaiaHas;
            this.Required = Required;
            this.WithField("?", "I'm missing permissions!");
            this.WithField("What can you do?", "Grant the permissions listed here to use this command.");
            List<string> Display = new();
            GuildPermission[] MissingGP = this.Required.Where(P => !this.KaiaHas.Has(P)).ToArray();
            foreach (GuildPermission P in MissingGP)
            {
                Display.Add($"{Emotes.Customs.KaiaDot}{Regex.Replace(P.ToString(), "([A-Z])", " $1").ToLower(CultureInfo.InvariantCulture)}");
            }
            this.WithListWrittenToField($"Permissions Required", Display, "\n");
        }

        public string GuildName { get; }

        public string CommandName { get; }

        public GuildPermissions KaiaHas { get; }

        public GuildPermission[] Required { get; }
    }
}
