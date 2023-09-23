using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class KaiaLocationEvent(string Message, double Weight, KaiaUserReward RewardResult)
    {
        public string Message { get; protected set; } = Message;

        public double Weight { get; } = Weight;

        public KaiaUserReward RewardResult { get; protected set; } = RewardResult;

        public KaiaLocationExplorationStatus Status { get; set; } = KaiaLocationExplorationStatus.NotDoneYet;
    }
}