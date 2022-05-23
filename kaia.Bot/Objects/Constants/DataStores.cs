using izolabella.Storage.Objects.DataStores;

namespace Kaia.Bot.Objects.Constants
{
    internal static class DataStores
    {
        internal static DataStore UserStore => new(Strings.App.Name, Strings.DataStoreNames.UserStore);
        internal static DataStore GuildStore => new(Strings.App.Name, Strings.DataStoreNames.GuildStore);
    }
}
