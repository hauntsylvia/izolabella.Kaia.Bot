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
        public RoseEvent(double Weight) : base(ExplorationStrings.RoseEvent.Message,
            Weight,
            new(0, new Rose()))
        {
        }
    }
}
