using Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
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
                    new NotebookEvent(10),
                }, TimeSpan.FromHours(6), TimeSpan.FromHours(16), TimeSpan.FromHours(24), new("💮"), new("https://i.pinimg.com/originals/1d/89/6a/1d896ab3d33457c4b50befc3a2a342b9.gif")),
            #endregion

            #region dark meadow
            new("Dark Meadow", "A meadow, retreating its otherwise bright and preppy aesthetic in favor of being drowned in the moon's neutral tones.", "A peaceful meadow.",
                true, 63020222035, new List<KaiaLocationEvent>()
                {
                    new NotebookEvent(10),
                    new RoseEvent(5),
                }, AvailableAt: TimeSpan.FromHours(16), AvailableTo: TimeSpan.FromHours(7), TimeSpan.FromHours(24), new("🌙"), new("https://i.pinimg.com/originals/33/4e/a1/334ea17c92dfccd6418b3ebe9206aaa7.gif")),
            #endregion

            #region factory
            new("Factory", "A long forgotten vessel of machinery, boasting massive towers and numerous floors. Whether or not you are the only one here is up for debate, but maybe you'll find something useful enough that it was worth your time.",
                "An abandoned facility once used to create wonders of machines.", true, 7120221753, new List<KaiaLocationEvent>()
                {
                    new DeadFingerEvent(1),
                }, TimeSpan.FromHours(22.5), TimeSpan.FromHours(6), TimeSpan.FromHours(36), new("🏭"), null)
            #endregion
        };
    }
}
