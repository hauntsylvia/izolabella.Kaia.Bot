using Kaia.Bot.Objects.Constants.Exploration;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class CigaretteKickNutAndBoltEvent : KaiaLocationEvent
    {
        public CigaretteKickNutAndBoltEvent(double Weight) : base(ExplorationStrings.CigaretteKickNutAndBoltEvent.Message,
            Weight,
            new(0, new Cigarette(), new NutAndBolt()))
        {
        }
    }
}
