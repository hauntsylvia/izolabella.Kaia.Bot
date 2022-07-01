using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kaia.Bot.Objects.Discord.Components.KaiaButton;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationView : KaiaItemContentView
    {
        public LocationView(LocationViewPaginated? From, KaiaLocation Location, CommandContext Context, KaiaUser U) : base(From, Context, true)
        {
            this.U = U;
            this.Location = Location;
            this.ExploreButton = new(Context, "Explore", Emotes.Counting.Explore);
            this.ExploreButton.OnButtonPush += this.ExploreAsync;
        }

        public KaiaUser U { get; }

        public KaiaLocation Location { get; }

        public KaiaButton ExploreButton { get; }

        private async Task ExploreAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            KaiaLocation? Loc = (await UserWhoPressed.LocationProcessor.GetUserLocationsExploredAsync()).FirstOrDefault(X => X.Id == this.Location.Id) ?? KaiaLocationRoom.Locations.FirstOrDefault(X => X.Id == this.Location.Id);
            if (Loc != null)
            {
                await Arg.DeferAsync(true);
                KaiaLocationEvent Result = await Loc.ExploreAsync(UserWhoPressed);
                await UserWhoPressed.LocationProcessor.AddLocationExploredAsync(Loc);

                KaiaButton OK = new(this.Context, "Ok", null, false);
                ButtonExecHandler? A = null;
                A = async (Arg, P) =>
                {
                    if(A != null)
                    {
                        OK.OnButtonPush -= A;
                    }
                    await Arg.DeferAsync(true);
                    await this.StartAsync(UserWhoPressed);
                };
                OK.OnButtonPush += A;
                await this.Context.UserContext.ModifyOriginalResponseAsync(A =>
                {
                    A.Embed = new LocationExplorationResult(this.Location, Result).Build();
                    A.Components = new ComponentBuilder().WithButton(OK).Build();
                    A.Content = null;
                });
                this.Dispose();
            }
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser U)
        {
            LocationRawView V = new(this.Context, this.Location);
            await V.RefreshAsync();
            return V;
        }

        public async Task<ComponentBuilder> GetComponentsAsync(KaiaUser U)
        {
            KaiaLocation? UserBoundLocation = (await U.LocationProcessor.GetUserLocationsExploredAsync()).FirstOrDefault(A => A.Id == this.Location.Id);
            ComponentBuilder CB = (await this.GetDefaultComponents()).WithButton(this.ExploreButton.WithDisabled((UserBoundLocation ?? this.Location).Status != KaiaLocationExplorationStatus.Successful));
            return CB;
        }

        public override async Task StartAsync(KaiaUser U)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
            ComponentBuilder Com = await this.GetComponentsAsync(U);
            await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
