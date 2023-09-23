using izolabella.Kaia.Bot.Objects.Constants.Exploration;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class NotebookEvent(double Weight) : KaiaLocationEvent(ExplorationStrings.NotebookEvent.Message,
        Weight,
        new(0, new Notebook()))
    {
    }
}