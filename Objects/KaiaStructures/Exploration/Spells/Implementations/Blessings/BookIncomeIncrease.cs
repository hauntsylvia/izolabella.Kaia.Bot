using Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Implementations.Blessings
{
    public class BookIncomeIncrease : Spell
    {
        public BookIncomeIncrease() : base("Book Income Increaser",
                                          "Increases book income by a random multiplier between 1 and 1.99.",
                                          false,
                                          Emotes.Counting.Blessings,
                                          new(TimeSpan.FromHours(4), 7120222211))
        {
        }

        public static double MultiplyBy => new Random().NextDouble() + 1;
    }
}
