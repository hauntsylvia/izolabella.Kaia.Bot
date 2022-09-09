using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Constants.Responses;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;

public class LocationViewPaginated : KaiaPathEmbedPaginated
{
    public LocationViewPaginated(CommandContext Context)
        : base(new(), Context, 0, Strings.EmbedStrings.FakePaths.Outside, Strings.EmbedStrings.FakePaths.Locations)
    {
        this.U = new(Context.UserContext.User.Id);
        ItemSelected += this.ItemSelectedAsync;
    }

    KaiaUser U { get; }

    protected override Task ClientRefreshAsync()
    {
        IEnumerable<KaiaLocation> Inventory = KaiaLocationRoom.Locations;

        foreach (KaiaLocation[] Chunk in Inventory.Where(IB => IB.StatusNoTimeout == KaiaLocationExplorationStatus.Successful).OrderBy(IB => IB.AvailableUntil.HasValue ? IB.AvailableUntil : IB.End).Chunk(4))
        {
            LocationPage Embed = new(Chunk, this.U);
            List<SelectMenuOptionBuilder> B = new();
            foreach (KaiaLocation Location in Chunk)
            {
                B.Add(new(Location.Name, Location.Id.ToString(CultureInfo.InvariantCulture), null, Location.Emote, false));
            }
            this.EmbedsAndOptions.Add(Embed, B);
        }

        return Task.CompletedTask;
    }

    private async void ItemSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
    {
        string? F = ItemsSelected.FirstOrDefault();
        if (F != null && ulong.TryParse(F, out ulong Res))
        {
            KaiaLocation? Location = await DataStores.GetUserLocationsStore(Component.User.Id).ReadAsync<KaiaLocation>(Res) ?? KaiaLocationRoom.Locations.FirstOrDefault(KL => KL.Id == Res);
            if (Location != null)
            {
                LocationView V = new(this, Location, this.Context, this.U);
                await Component.DeferAsync(true);
                this.Dispose();
                await V.StartAsync(this.U);
            }
            else
            {
                await Responses.PipeErrors(this.Context, new SingleItemNotFound());
            }
        }
    }
}
