using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.KaiaAchievementRoom
{
    public class KaiaAchievementRoom
    {
        public static List<KaiaAchievement> Achievements => new()
        {
            new CountAchievement(1, 1000, "You've made it to 1 and three 0s.", true),
            new CountAchievement(2, 10000, "You've made it to 1 and *four* 0s. Have you considered doing anything else with your life?", true),
            
            new CountAchievement(3, 1000, "You've counted a total of 1000 numbers. Congrats!", false),
            new CountAchievement(4, 10000, "You've counted up to 10000 total numbers. Have you considered touching grass?", false),
        };
    }
}
