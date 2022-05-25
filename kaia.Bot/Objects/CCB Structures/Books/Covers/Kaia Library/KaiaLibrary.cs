using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Books.Covers.Implementations
{
    public class KaiaLibrary
    {
        public static List<KaiaBook> Books => new()
        {
            new("Somokuto", "Santōka Taneda", 5, 0.185, 1.0005, 5, 2, new(2022, 7, 24), 5252022232),
            new("Dogra Magra", "Yumeno Kyūsaku", 505, 0.1, 1.122, 10, 1.95, new(2024, 7, 24), 5252022232),
        };

        public static KaiaBook? GetActiveBookById(string Id)
        {
            return KaiaLibrary.Books.FirstOrDefault(B => B.BookId == Id);
        }
    }
}
