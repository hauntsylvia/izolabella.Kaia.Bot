using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Exceptions
{
    public class KaiaSaleListingInvalidException(KaiaUser? UserTryingToSell) : KaiaException($"Listing of item invalid due to {(UserTryingToSell == null ? "Kaia not being able to sell the item." : "the item not being available for sale through users.")}")
    {
    }
}