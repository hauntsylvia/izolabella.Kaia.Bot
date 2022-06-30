namespace Kaia.Bot.Objects.KaiaStructures.Users
{
    public class KaiaUserReward
    {
        public KaiaUserReward(double Petals, params KaiaInventoryItem[] Items)
        {
            this.Petals = Petals;
            this.Items = Items;
        }

        public double Petals { get; }
        public KaiaInventoryItem[] Items { get; }
    }
}
