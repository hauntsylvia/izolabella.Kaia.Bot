using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Items;

public class StoreCommand : KaiaCommand
{
    public override string Name => "Store";

    public override string Description => "Display the global store, or buy an item.";

    public override bool GuildsOnly => false;

    public override List<IzolabellaCommandParameter> Parameters { get; } = new()
    {
        new("User Relationships", "Whether to include user listings or not.", ApplicationCommandOptionType.Boolean, false),
        new("Lister", "Filters all listings by this user.", ApplicationCommandOptionType.User, false)
    };

    public override List<IIzolabellaCommandConstraint> Constraints { get; } = new();

    public override List<GuildPermission> RequiredPermissions { get; } = new();

    public override string ForeverId => CommandForeverIds.StoreOrBuy;

    public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
    {
        IzolabellaCommandArgument? IncludeUserListings = Arguments.FirstOrDefault(A => A.Name == "user-listings");
        IzolabellaCommandArgument? Lister = Arguments.FirstOrDefault(A => A.Name == "lister");
        IUser? User = null;
        if (Lister != null && Lister.Value is IUser U)
        {
            User = U;
        }
        ItemsPaginated E = new(Context, User, IncludeUserListings?.Value is not bool B || B);
        await E.StartAsync();
    }
}
