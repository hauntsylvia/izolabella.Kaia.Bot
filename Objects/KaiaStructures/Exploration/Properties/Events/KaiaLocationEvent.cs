using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class KaiaLocationEvent
    {
        public KaiaLocationEvent(string Message, double Weight, KaiaUserReward RewardResult)
        {
            this.Message = Message;
            this.Weight = Weight;
            this.RewardResult = RewardResult;
        }

        public string Message { get; protected set; }

        public double Weight { get; }

        public KaiaUserReward RewardResult { get; protected set; }

        public KaiaLocationExplorationStatus Status { get; set; } = KaiaLocationExplorationStatus.NotDoneYet;
    }
}
