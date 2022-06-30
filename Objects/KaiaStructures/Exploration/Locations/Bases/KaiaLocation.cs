namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Bases
{
    public class KaiaLocation
    {
        public KaiaLocation(string Name, IEnumerable<KaiaUserReward> Rewards, TimeSpan AvailableFrom, TimeSpan AvailableTo, Uri CoverUrl)
        {
            this.Name = Name;
            this.Rewards = Rewards;
            this.AvailableFrom = AvailableFrom;
            this.AvailableTo = AvailableTo;
            this.CoverUrl = CoverUrl;
        }

        public string Name { get; }

        public IEnumerable<KaiaUserReward> Rewards { get; }

        public TimeSpan AvailableFrom { get; }

        public TimeSpan AvailableTo { get; }

        public Uri CoverUrl { get; }
    }
}
