using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Locations
{
    public class KaiaLocationRoom
    {
        public static IEnumerable<KaiaLocation> Locations => new List<KaiaLocation>()
        {
            #region bright meadow
            new("Bright Meadow", "A meadow, graceful though helpess amidst the sun.", "A peaceful meadow.",
                true, 63020222001, new List<KaiaLocationEvent>()
                {
                    new("a butterfly! . . oh, u've killed it. at least now u have a notebook to write down ur regrets.",
                        10, new(0, new Notebook()))
                }, TimeSpan.FromHours(6.5), TimeSpan.FromHours(16), TimeSpan.FromHours(24), new("https://i.pinimg.com/originals/1d/89/6a/1d896ab3d33457c4b50befc3a2a342b9.gif")),
            #endregion

            #region dark meadow
            new("Dark Meadow", "A meadow, retreating its otherwise bright and preppy aesthetic in favor of being drowned in the moon's neutral tones.", "A peaceful meadow.",
                true, 63020222035, new List<KaiaLocationEvent>()
                {
                    new("a butterfly! . . oh, u've killed it. at least now u have a notebook to write down ur regrets.",
                        10, new(0, new Notebook())),
                    new("u've found a rose. be careful picking it up!",
                        5, new(0, new Rose()))
                }, TimeSpan.FromHours(16), TimeSpan.FromHours(6.5), TimeSpan.FromHours(24), new("https://i.pinimg.com/originals/33/4e/a1/334ea17c92dfccd6418b3ebe9206aaa7.gif"))
            {
                Overnight = true
            }
            #endregion
        };
    }
}
