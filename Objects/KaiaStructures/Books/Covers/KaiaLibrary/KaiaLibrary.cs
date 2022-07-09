using izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary
{
    public class KaiaLibrary
    {
        public static List<KaiaBook> Books => new()
        {
            new("Somokuto",
                "Santōka Taneda",
                Pages: 5,
                Starting: 0.185,
                ExponentToIncreaseBy: 1.0005,
                CostPerPage: 5,
                CostPerPageExponent: 2,
                AvailableUntil: new(year: 2022, month: 7, day: 24),
                5252022232),

            new("Dogra Magra",
                "Yumeno Kyūsaku",
                Pages: 504,
                Starting: 0.1,
                ExponentToIncreaseBy: 1.122,
                CostPerPage: 10,
                CostPerPageExponent: 1.95,
                AvailableUntil: new(year: 2022, month: 9, day: 24),
                52520220411),

            new("La Porte Étroite",
                "André Gide",
                Pages: 112,
                Starting: 0.11,
                ExponentToIncreaseBy: 1.0001,
                CostPerPage: 6,
                CostPerPageExponent: 1.5,
                AvailableUntil: new(year: 2022, month: 10, day: 1),
                52520220415),

            new("Infinite Potential: The Greatest Works of Neville Goddard",
                "Neville Goddard",
                Pages: 283,
                Starting: 0.02,
                ExponentToIncreaseBy: 1.1,
                CostPerPage: 20,
                CostPerPageExponent: 1.5,
                AvailableUntil: new(year: 2022, month: 11, day: 27),
                52720220319),

            new("Do Androids Dream of Electric Sheep?",
                "Philip K. Dick",
                Pages: 244,
                Starting: 0.11,
                ExponentToIncreaseBy: 1.00025,
                CostPerPage: 6,
                CostPerPageExponent: 1.5001,
                AvailableUntil: new(year: 2023, month: 1, day: 1),
                62720220416),

            new("Fahrenheit 451",
                "Ray Bradbury",
                Pages: 194,
                Starting: 0.05,
                ExponentToIncreaseBy: 1.1,
                CostPerPage: 12,
                CostPerPageExponent: 1.6,
                AvailableUntil: new(year: 2023, month: 1, day: 18),
                62720220418),
        };

        public static KaiaBook? GetActiveBookById(string Id)
        {
            return Books.FirstOrDefault(B => B.BookId == Id);
        }
    }
}
