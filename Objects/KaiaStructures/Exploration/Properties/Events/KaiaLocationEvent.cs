using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
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
