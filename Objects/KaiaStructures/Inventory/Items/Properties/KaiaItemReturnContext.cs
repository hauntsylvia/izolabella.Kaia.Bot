﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties
{
    public class KaiaItemReturnContext
    {
        public KaiaItemReturnContext(string Message, KaiaUserReward? Reward)
        {
            this.Message = Message;
            this.Reward = Reward;
        }

        public string Message { get; }

        public KaiaUserReward? Reward { get; }
    }
}