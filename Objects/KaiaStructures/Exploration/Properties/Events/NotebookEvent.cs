using izolabella.Kaia.Bot.Objects.Constants.Exploration;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;

public class NotebookEvent : KaiaLocationEvent
{
    public NotebookEvent(double Weight) : base(ExplorationStrings.NotebookEvent.Message,
        Weight,
        new(0, new Notebook()))
    {
    }
}
