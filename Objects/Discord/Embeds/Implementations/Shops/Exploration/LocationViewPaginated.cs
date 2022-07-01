using Kaia.Bot.Objects.Constants.Responses;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationViewPaginated : KaiaPathEmbedPaginated
    {
        public LocationViewPaginated(CommandContext Context)
            : base(new(), Context, 0, Strings.EmbedStrings.FakePaths.Outside, Strings.EmbedStrings.FakePaths.Locations)
        {
            this.U = new(Context.UserContext.User.Id);
            this.ItemSelected += this.ItemSelectedAsync;
        }

        KaiaUser U { get; }

        protected override Task ClientRefreshAsync()
        {
            IEnumerable<KaiaLocation> Inventory = KaiaLocationRoom.Locations;

            foreach (KaiaLocation[] Chunk in Inventory.Where(IB => IB.StatusNoTimeout == KaiaStructures.Exploration.Locations.Enums.KaiaLocationExplorationStatus.Successful).OrderBy(IB => IB.AvailableUntil).Chunk(4))
            {
                LocationPage Embed = new(Chunk, this.U);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaLocation Location in Chunk)
                {
                    B.Add(new(Location.Name, Location.Id.ToString(CultureInfo.InvariantCulture), null, Emotes.Counting.Location, false));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            return Task.CompletedTask;
        }

        private async void ItemSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            string? F = ItemsSelected.FirstOrDefault();
            if(F != null && ulong.TryParse(F, out ulong Res))
            {
                KaiaLocation? Location = await DataStores.GetUserLocationsStore(Component.User.Id).ReadAsync<KaiaLocation>(Res) ?? KaiaLocationRoom.Locations.FirstOrDefault(KL => KL.Id == Res);
                if(Location != null)
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
}
