using Kaia.Bot.Objects.Constants.Exploration;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class DeadFingerEvent : KaiaLocationEvent
    {
        public DeadFingerEvent(double Weight) : base(ExplorationStrings.DeadFingerEvent.Message,
            Weight,
            new(0, new DeadFinger()))
        {
        }
    }
}
