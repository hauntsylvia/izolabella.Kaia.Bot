using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties
{
    public class KaiaItemReturnContext(string Message, KaiaUserReward? Reward)
    {
        public string Message { get; } = Message;

        public KaiaUserReward? Reward { get; } = Reward;
    }
}