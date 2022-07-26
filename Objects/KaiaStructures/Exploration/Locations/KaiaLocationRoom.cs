using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Properties.Events;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Locations
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
                }, TimeSpan.FromHours(6), TimeSpan.FromHours(16), TimeSpan.FromHours(16), new("💮"), new("https://i.pinimg.com/originals/1d/89/6a/1d896ab3d33457c4b50befc3a2a342b9.gif")),
            #endregion

            #region dark meadow
            new("Dark Meadow", "A meadow, retreating its otherwise bright and preppy aesthetic in favor of being drowned in the moon's neutral tones.", "A peaceful meadow.",
                true, 63020222035, new List<KaiaLocationEvent>()
                {
                    new NotebookEvent(10),
                    new RoseEvent(5),
                }, AvailableAt: TimeSpan.FromHours(16), AvailableTo: TimeSpan.FromHours(7), TimeSpan.FromHours(16), new("🌙"),
                CoverUrl: new("https://i.pinimg.com/originals/33/4e/a1/334ea17c92dfccd6418b3ebe9206aaa7.gif"),
                CoverUrlCredit: new("https://willow95-deactivated20220224.tumblr.com/post/625648064600981504")),
            #endregion

            #region factory
            new("Factory", "A long forgotten vessel of machinery, boasting massive towers and numerous floors. Whether or not you are the only one here is up for debate, but maybe you'll find something useful enough that it was worth your time.",
                "An abandoned facility once used to create wonders of machines.", true, 7120221753, new List<KaiaLocationEvent>()
                {
                    new DeadFingerEvent(1),
                    new CigaretteKickNutAndBoltEvent(5),
                }, TimeSpan.FromHours(13), TimeSpan.FromHours(24), TimeSpan.FromHours(6), new("🏭"), null),
            #endregion

            #region quiet town
            new(Name: "Quiet Town",
                Description: "A cute and quiet town. Everything here is oddly still.",
                ShortDescription: "A cute little town; home to many.",
                DisplayRewards: true,
                SuperSecretSelfId: 7220220406,
                Events: new List<KaiaLocationEvent>()
                {
                    new DeadFingerEvent(0.01),
                    new NotebookEvent(0.15),
                    new CandleEvent(0.7),
                },
                AvailableAt: TimeSpan.FromHours(15),
                AvailableTo: TimeSpan.FromHours(22.25),
                MinimumTimeBetweenExplorations: TimeSpan.FromHours(9),
                Emote: new("🕯️"),
                CoverUrl: new("https://i.pinimg.com/originals/c0/36/28/c03628e7339e0d492cdd077acb6a9e8f.gif"),
                CoverUrlCredit: new("https://steamcommunity.com/sharedfiles/filedetails/?id=1624054751&searchtext=")),
            #endregion

            #region autumn cottage
            new("Autumn Cottage", "A small cottage planted deep within the woods. The location is unknown.",
                "A small cottage planted deep within the woods. The location is unknown.", true, 7220220500, new List<KaiaLocationEvent>()
                {
                    new CandleEvent(0.3, false),
                    new RoseEvent(0.6),
                    new NotebookEvent(1),
                }, TimeSpan.FromHours(2.5), TimeSpan.FromHours(11), TimeSpan.FromHours(9), new("🍂"),
                CoverUrl: new("https://i.pinimg.com/564x/d9/e3/f3/d9e3f308a0908fedff694bf3b23627e2.jpg"),
                CoverUrlCredit: new("https://old.reddit.com/r/PixelArt/comments/di68y1/autumn_scenery/")),
            #endregion

            #region empty street
            new("Empty Street", "An empty street late at night. What awaits anyone here that's important enough to be out so late and alone?",
                "An empty street late at night.", true, 7220220514, new List<KaiaLocationEvent>()
                {
                    new CandleEvent(0.9, false),
                    new CigaretteKickNutAndBoltEvent(0.25)
                }, TimeSpan.FromHours(19), TimeSpan.FromHours(4), TimeSpan.FromHours(4), new("🌙"),
                CoverUrl: new("https://i.pinimg.com/originals/7a/f8/ce/7af8ced6fc14a1f2840b72187ba19248.gif"),
                CoverUrlCredit: new("https://guttykreum.tumblr.com/post/182656493636/%E7%AA%93-1am-tama-tokyo")),
            #endregion

            #region walmart

            //new(DisplayName: "Walmart",
            //    Description: "A super-store run by a mass of both fresh-out-of-highschool students and grumpy people that suck off management.",
            //    ShortDescription: "A super-store containing a diverse selection of items.",
            //    DisplayRewards: true,
            //    SuperSecretSelfId: 7720221639,
            //    Events: new List<KaiaLocationEvent>()
            //    {
            //        new DeadFingerEvent(0.09),
            //        new NotebookEvent(0.2),
            //        new CandleEvent(0.2),
            //        new KaiaLocationEvent("u didn't steal cigarettes.", 0.3, new(0, new Cigarette())),
            //        new RoseEvent(0.1),
            //    },
            //    AvailableAt: TimeSpan.FromHours(4),
            //    AvailableTo: TimeSpan.FromHours(23),
            //    MinimumTimeBetweenExplorations: TimeSpan.FromHours(24),
            //    Emote: new("🛒"),
            //    CoverUrl: new("https://i.pinimg.com/originals/c0/36/28/c03628e7339e0d492cdd077acb6a9e8f.gif"),
            //    CoverUrlCredit: new("https://steamcommunity.com/sharedfiles/filedetails/?id=1624054751&searchtext=")),
            #endregion
        };
    }
}
