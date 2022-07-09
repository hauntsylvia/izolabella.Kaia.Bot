using izolabella.Kaia.Bot.Objects.Constants.Exploration;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class RoseEvent : KaiaLocationEvent
    {
        public RoseEvent(double Weight) : base(ExplorationStrings.RoseEvent.Message,
            Weight,
            new(0, new Rose()))
        {
        }
    }
}
