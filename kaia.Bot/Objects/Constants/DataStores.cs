using izolabella.Storage.Objects.DataStores;

namespace Kaia.Bot.Objects.Constants
{
    internal static class DataStores
    {
        private static JsonSerializerSettings SerializerSettings => new()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        internal static DataStore UserStore => new(Strings.App.Name, Strings.DataStoreNames.UserStore, SerializerSettings);
        internal static DataStore GuildStore => new(Strings.App.Name, Strings.DataStoreNames.GuildStore, SerializerSettings);
    }
}
