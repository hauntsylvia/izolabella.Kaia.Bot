﻿using izolabella.Kaia.Bot.Objects.Constants.Exploration;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events
{
    public class CandleEvent : KaiaLocationEvent
    {
        public CandleEvent(double Weight, bool Ominous = true) : base(Ominous ? ExplorationStrings.CandleEvent.OminousMessage : ExplorationStrings.CandleEvent.Message,
            Weight,
            new(0, new Candle(), new Candle(), new Candle()))
        {
        }
    }
}