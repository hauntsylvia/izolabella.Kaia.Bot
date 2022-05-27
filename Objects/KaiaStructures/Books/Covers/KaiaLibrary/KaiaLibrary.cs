using Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases;

namespace Kaia.Bot.Objects.KaiaStructures.Books.Covers.Implementations
{
    public class KaiaLibrary
    {
        public static List<KaiaBook> Books => new()
        {
            new("Somokuto", "Santōka Taneda", 5, 0.185, 1.0005, 5, 2, new(2022, 7, 24), 5252022232),
            new("Dogra Magra", "Yumeno Kyūsaku", 504, 0.1, 1.122, 10, 1.95, new(2022, 9, 24), 52520220411),
            new("La Porte Étroite", "André Gide", 112, 0.11, 1.0001, 6, 1.5, new(2022, 10, 1), 52520220415),
        };

        public static KaiaBook? GetActiveBookById(string Id)
        {
            return KaiaLibrary.Books.FirstOrDefault(B => B.BookId == Id);
        }
    }
}
