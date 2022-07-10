using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using static izolabella.Kaia.Bot.Objects.Discord.Components.KaiaButton;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationView : KaiaItemContentView
    {
        public LocationView(LocationViewPaginated? From, KaiaLocation Location, CommandContext Context, KaiaUser U) : base(From, Context, true)
        {
            this.U = U;
            this.Location = Location;
            ExploreButton = new(Context, "Explore", Emotes.Counting.Explore);
            ExploreButton.OnButtonPush += ExploreAsync;
        }

        public KaiaUser U { get; }

        public KaiaLocation Location { get; }

        public KaiaButton ExploreButton { get; }

        public static async Task<KaiaLocation?> GetKaiaLocationAsync(ulong FromId, KaiaUser U)
        {
            KaiaLocation? UserLocation = (await U.LocationProcessor.GetUserLocationsExploredAsync()).FirstOrDefault(X => X.Id == FromId) ?? KaiaLocationRoom.Locations.FirstOrDefault(X => X.Id == FromId);
            if (UserLocation != null)
            {
                KaiaLocation? KaiaLocation = KaiaLocationRoom.Locations.FirstOrDefault(KL => KL.Id == UserLocation.Id);
                return KaiaLocation;
            }
            return null;
        }

        private async Task ExploreAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            KaiaLocation? Location = await GetKaiaLocationAsync(this.Location.Id, UserWhoPressed);
            if (Location != null)
            {
                await Arg.DeferAsync(true);
                KaiaLocationEvent Result = await Location.ExploreAsync(UserWhoPressed);
                await UserWhoPressed.LocationProcessor.AddLocationExploredAsync(Location);

                KaiaButton OK = new(Context, "Ok", null, false);
                ButtonExecHandler? A = null;
                A = async (Arg, P) =>
                {
                    if (A != null)
                    {
                        OK.OnButtonPush -= A;
                    }
                    await Arg.DeferAsync(true);
                    await StartAsync(UserWhoPressed);
                };
                OK.OnButtonPush += A;
                await Context.UserContext.ModifyOriginalResponseAsync(A =>
                {
                    A.Embed = new LocationExplorationResult(this.Location, Result).Build();
                    A.Components = new ComponentBuilder().WithButton(OK).Build();
                    A.Content = null;
                });
                Dispose();
            }
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            LocationRawView V = new(Context, Location);
            await V.RefreshAsync();
            return V;
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            KaiaLocation? UserBoundLocation = (await U.LocationProcessor.GetUserLocationsExploredAsync()).FirstOrDefault(A => A.Id == Location.Id);
            ComponentBuilder CB = (await GetDefaultComponents()).WithButton(ExploreButton.WithDisabled((UserBoundLocation ?? Location).Status != KaiaLocationExplorationStatus.Successful));
            return CB;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!Context.UserContext.HasResponded)
            {
                await Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await GetEmbedAsync(U);
            ComponentBuilder Com = await GetComponentsAsync(U);
            await Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            ExploreButton.Dispose();
        }
    }
}
