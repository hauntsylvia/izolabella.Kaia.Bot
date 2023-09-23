using izolabella.Kaia.Bot.Objects.Constants.Exploration;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class CigaretteKickNutAndBoltEvent(double Weight) : KaiaLocationEvent(ExplorationStrings.CigaretteKickNutAndBoltEvent.Message,
        Weight,
        new(0, new Cigarette(), new NutAndBolt()))
    {
    }
}