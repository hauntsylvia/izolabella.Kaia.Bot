using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.KaiaAchievementRoom
{
    public class KaiaAchievementRoom
    {
        public static List<KaiaAchievement> Achievements => new()
    {
        new CountAchievement(1, 1000, "You've made it to 1 and three 0s.", "Count to at least 1000.", true),
        new CountAchievement(2, 10000, "You've made it to 1 and *four* 0s. Have you considered doing anything else with your life?", "Count to at least 10000", true),

        new CountAchievement(3, 1000, "You've counted a total of 1000 numbers. Congrats!", "Count a total of 1000 numbers.", false),
        new CountAchievement(4, 10000, "You've counted up to 10000 total numbers. Have you considered touching grass?", "Count a total of 10000 numbers.", false),
    };
    }
}