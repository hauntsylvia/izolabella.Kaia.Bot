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
            new("Somokuto", "Santōka Taneda", 5, 3.36, 1.2, 500, 1.2125, DateTime.UtcNow.AddMonths(2)),
        };
    }
}
