namespace Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Implementations.Blessings
{
    public class BookIncomeIncrease : Spell
    {
        public BookIncomeIncrease() : base("Book Income Increaser",
                                          "Increases book income by a small random multiplier.",
                                          false,
                                          Emotes.Counting.Blessings,
                                          new(TimeSpan.FromHours(4), 7120222211))
        {
        }

        public static double MultiplyBy => new Random().NextDouble() + 1;
    }
}
