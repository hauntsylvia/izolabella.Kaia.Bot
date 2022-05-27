using izolabella.Storage.Objects.DataStores;

namespace Kaia.Bot.Objects.Constants
{
    internal static class DataStores
    {
        private static JsonSerializerSettings SerializerSettings => new();
        internal static DataStore UserStore => new(Strings.App.Name, Strings.DataStoreNames.UserStore, SerializerSettings);
        internal static DataStore GuildStore => new(Strings.App.Name, Strings.DataStoreNames.GuildStore, SerializerSettings);
        internal static DataStore UserBookStore => new(Strings.App.Name, Strings.DataStoreNames.BookStore, SerializerSettings);
        internal static DataStore GetUserBookStore(ulong UserId)
        {
            DataStore DS = UserBookStore;
            DS.MakeSubStore(UserId.ToString(CultureInfo.InvariantCulture));
            return DS;
        }
    }
}
