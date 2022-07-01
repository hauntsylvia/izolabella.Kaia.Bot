using Kaia.Bot.Objects.Constants.Exploration;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class RoseEvent : KaiaLocationEvent
    {
        public RoseEvent(int Weight) : base("a butterfly! . . oh, u've killed it. at least now u have a notebook to write down ur regrets.",
            Weight,
            new(0, new Notebook()))
        {
            this.Message = ExplorationStrings.NotebookEvent.Message;
        }
    }
}
