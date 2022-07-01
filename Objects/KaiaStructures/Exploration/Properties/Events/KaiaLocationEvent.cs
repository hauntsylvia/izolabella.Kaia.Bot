using Kaia.Bot.Objects.KaiaStructures.Exploration.Locations.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class KaiaLocationEvent
    {
        public KaiaLocationEvent(string Message, int Weight, KaiaUserReward RewardResult)
        {
            this.Message = Message;
            this.Weight = Weight;
            this.RewardResult = RewardResult;
        }

        public string Message { get; protected set; }

        public int Weight { get; }

        public KaiaUserReward RewardResult { get; protected set; }

        public KaiaLocationExplorationStatus Status { get; set; } = KaiaLocationExplorationStatus.NotDoneYet;
    }
}
