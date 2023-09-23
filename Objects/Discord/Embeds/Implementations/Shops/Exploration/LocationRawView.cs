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
            this.UserLocation = Location;
        }

        public CommandContext Context { get; }

        public KaiaLocation UserLocation { get; set; }

        private KaiaUser User => new(this.Context.UserContext.User.Id);

        private KaiaLocation? KaiaLocation { get; set; }

        protected override async Task ClientRefreshAsync()
        {
            this.UserLocation = (await new KaiaUser(this.Context.UserContext.User.Id).LocationProcessor.GetUserLocationsExploredAsync()).FirstOrDefault(A => A.Id == this.UserLocation.Id) ?? this.UserLocation;
            this.KaiaLocation = await LocationView.GetKaiaLocationAsync(this.UserLocation.Id, this.User);
            if (this.KaiaLocation != null)
            {
                if (this.KaiaLocation.CoverUrl != null)
                {
                    this.WithImage(this.KaiaLocation.CoverUrl);
                }
                this.WithField("description", $"```{this.KaiaLocation.Description}```");
                if (this.UserLocation.MustWaitUntil != null && this.UserLocation.Status == KaiaLocationExplorationStatus.Timeout)
                {
                    double Hours = Math.Round((this.UserLocation.MustWaitUntil - DateTime.UtcNow).Value.TotalHours, 2);
                    this.WithField("timeout!", $"please wait for `{Hours}` {(Hours != 1 ? "hours" : "hour")} before exploring this place again!");
                }
                if (this.KaiaLocation.DisplayRewards)
                {
                    List<string> Display = new();
                    double TotalWeight = this.KaiaLocation.Events.Sum(KV => KV.Weight);
                    foreach (KaiaLocationEvent A in this.KaiaLocation.Events.OrderBy(KV => KV.Weight).Take(3))
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
                    this.WithListWrittenToField("rarest potential finds", Display, "");
                }
            }
        }
    }
}