using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Self;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeData;
using izolabella.LoFi.Server.Structures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Self
{
    public class SelfLoFiStats : KaiaCommand
    {
        public override string Name => "LoFi Stats";

        public override string Description => $"View various statistics regarding your LoFi listening.";

        public override bool GuildsOnly => false;

        public override List<IzolabellaCommandParameter> Parameters => new();

        public override List<GuildPermission> RequiredPermissions => new();

        public override string ForeverId => CommandForeverIds.SelfLoFiStats;

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            LoFiStats Stats = new(Context.UserContext.User.Id);
            await Stats.RefreshAsync();
            await Context.UserContext.RespondAsync(ephemeral: false, embed: Stats.Build());
        }
    }
}
