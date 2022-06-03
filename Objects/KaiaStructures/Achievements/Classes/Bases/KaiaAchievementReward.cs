using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases
{
    public class KaiaAchievementReward
    {
        public KaiaAchievementReward(double Petals, params KaiaInventoryItem[] Items)
        {
            this.Petals = Petals;
            this.Items = Items;
        }

        public double Petals { get; }
        public KaiaInventoryItem[] Items { get; }
    }
}
