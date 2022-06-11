using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.KaiaStructures.Books.Covers.KaiaLibrary
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
                AvailableUntil: new(2022, 7, 24),
                5252022232),

            new("Dogra Magra",
                "Yumeno Kyūsaku",
                Pages: 504,
                Starting: 0.1,
                ExponentToIncreaseBy: 1.122,
                CostPerPage: 10,
                CostPerPageExponent: 1.95,
                AvailableUntil: new(2022, 9, 24),
                52520220411),

            new("La Porte Étroite",
                "André Gide",
                Pages: 112,
                Starting: 0.11,
                ExponentToIncreaseBy: 1.0001,
                CostPerPage: 6,
                CostPerPageExponent: 1.5,
                AvailableUntil: new(2022, 10, 1),
                52520220415),

            new("Infinite Potential: The Greatest Works of Neville Goddard",
                "Neville Goddard",
                Pages: 283,
                Starting: 0.02,
                ExponentToIncreaseBy: 1.1,
                CostPerPage: 20,
                CostPerPageExponent: 1.5,
                AvailableUntil: new(2022, 11, 27),
                52720220319),
        };

        public static KaiaBook? GetActiveBookById(string Id)
        {
            return Books.FirstOrDefault(B => B.BookId == Id);
        }
    }
}
