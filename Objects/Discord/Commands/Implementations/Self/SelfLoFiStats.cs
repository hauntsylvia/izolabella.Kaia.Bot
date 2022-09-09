using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Discord.Objects.Structures.Discord.Commands;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Self;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.MeData;
using izolabella.LoFi.Server.Structures.Users;
using izolabella.Music.Structure.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Self;

public class SelfLoFiStats : KaiaCommand
{
    public override string Name => "LoFi Stats";

    public override string Description => $"View various statistics regarding your LoFi listening.";

    public override bool GuildsOnly => false;

    public override List<IzolabellaCommandParameter> Parameters => new()
    {
        new("Name", $"The name to save for your {izolabella.Util.Info.NameForPackages}.LoFi profile.", ApplicationCommandOptionType.String, false)
    };

    public override List<GuildPermission> RequiredPermissions => new();

    public override string ForeverId => CommandForeverIds.SelfLoFiStats;

    public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
    {
        IzolabellaCommandArgument? NewNameArg = Arguments.FirstOrDefault(A => A.Name == "name");
        if(NewNameArg != null && NewNameArg.Value is string NewName)
        {
            LoFiUser? U = await LoFiUser.Get(Context.UserContext.User.Id, DataStores.LoFiUserStore);
            if(U != null)
            {
                await U.Update(A => A.Profile.DisplayName = $"{NewName}", DataStores.LoFiUserStore);
            }
        }
        LoFiStats Stats = new(Context.UserContext.User.Id);
        await Stats.RefreshAsync();
        await Context.UserContext.RespondAsync(ephemeral: false, embed: Stats.Build());
    }
}
