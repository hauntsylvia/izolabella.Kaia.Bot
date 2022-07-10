using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration
{
    public class LocationRawView : KaiaPathEmbedRefreshable
    {
        public LocationRawView(CommandContext Context, KaiaLocation Location) : base(Strings.EmbedStrings.FakePaths.Outside, Location.Name)
        {
            this.Context = Context;
            UserLocation = Location;
        }

        public CommandContext Context { get; }

        public KaiaLocation UserLocation { get; set; }

        private KaiaUser User => new(Context.UserContext.User.Id);

        private KaiaLocation? KaiaLocation { get; set; }

        protected override async Task ClientRefreshAsync()
        {
            UserLocation = (await new KaiaUser(Context.UserContext.User.Id).LocationProcessor.GetUserLocationsExploredAsync()).FirstOrDefault(A => A.Id == UserLocation.Id) ?? UserLocation;
            KaiaLocation = await LocationView.GetKaiaLocationAsync(UserLocation.Id, User);
            if (KaiaLocation != null)
            {
                if (KaiaLocation.CoverUrl != null)
                {
                    WithImage(KaiaLocation.CoverUrl);
                }
                WithField("description", $"```{KaiaLocation.Description}```");
                if (UserLocation.MustWaitUntil != null && UserLocation.Status == KaiaLocationExplorationStatus.Timeout)
                {
                    double Hours = Math.Round((UserLocation.MustWaitUntil - DateTime.UtcNow).Value.TotalHours, 2);
                    WithField("timeout!", $"please wait for `{Hours}` {(Hours != 1 ? "hours" : "hour")} before exploring this place again!");
                }
                if (KaiaLocation.DisplayRewards)
                {
                    List<string> Display = new();
                    double TotalWeight = KaiaLocation.Events.Sum(KV => KV.Weight);
                    foreach (KaiaLocationEvent A in KaiaLocation.Events.OrderBy(KV => KV.Weight).Take(3))
                    {
                        string SubDisplay = string.Empty;
                        double Chance = A.Weight / TotalWeight * 100.0;
                        IEnumerable<KaiaInventoryItem> ItemsWithoutDuplicates = A.RewardResult.Items.GroupBy(X => X.DisplayName).Select(X => X.First());
                        foreach (KaiaInventoryItem Item in ItemsWithoutDuplicates)
                        {
                            SubDisplay += $"→ {Item.DisplayEmote} {Item.DisplayName} [{Math.Round(Chance, 2)}%]\n";
                        }
                        if (SubDisplay.Length > 0)
                        {
                            Display.Add(SubDisplay);
                        }
                    }
                    WithListWrittenToField("rarest potential finds", Display, "");
                }
            }
        }
    }
}
