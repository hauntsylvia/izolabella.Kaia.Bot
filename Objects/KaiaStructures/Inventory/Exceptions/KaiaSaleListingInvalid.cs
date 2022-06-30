namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Exceptions
{
    public class KaiaSaleListingInvalidException : KaiaException
    {
        public KaiaSaleListingInvalidException(List<KaiaInventoryItem> ItemsTryingToSell,
                                      KaiaUser? UserTryingToSell) : base($"Listing of item invalid due to {(UserTryingToSell == null ? "Kaia not being able to sell the item." : "the item not being available for sale through users.")}")
        {
        }
    }
}
