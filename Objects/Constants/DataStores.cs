using izolabella.Storage.Objects.DataStores;

namespace izolabella.Kaia.Bot.Objects.Constants
{
    internal static class DataStores
    {
        private static JsonSerializerSettings SerializerSettings => new();
        //{
        //    TypeNameHandling = TypeNameHandling.Auto
        //};
        internal static DataStore UserStore => new(Strings.App.Name, Strings.DataStoreNames.UserStore, SerializerSettings);
        internal static DataStore GuildStore => new(Strings.App.Name, Strings.DataStoreNames.GuildStore, SerializerSettings);

        #region books

        private static DataStore UserBooksMainDirectory => new(Strings.App.Name, Strings.DataStoreNames.BookStore, SerializerSettings);
        internal static DataStore GetUserBookStore(ulong UserId)
        {
            DataStore DS = UserBooksMainDirectory;
            DS.MakeSubStore(UserId.ToString(CultureInfo.InvariantCulture));
            return DS;
        }

        #endregion

        #region achievements

        private static DataStore UserAchievementsMainDirectory => new(Strings.App.Name, Strings.DataStoreNames.AchievementStore, SerializerSettings);
        internal static DataStore GetUserAchievementStore(ulong UserId)
        {
            DataStore DS = UserAchievementsMainDirectory;
            DS.MakeSubStore(UserId.ToString(CultureInfo.InvariantCulture));
            return DS;
        }

        #endregion

        #region locations

        private static DataStore UserLocationsMainDirectory => new(Strings.App.Name, Strings.DataStoreNames.LocationStore, SerializerSettings);
        internal static DataStore GetUserLocationsStore(ulong UserId)
        {
            DataStore DS = UserLocationsMainDirectory;
            DS.MakeSubStore(UserId.ToString(CultureInfo.InvariantCulture));
            return DS;
        }

        #endregion

        #region spells

        private static DataStore UserSpellsMainDirectory => new(Strings.App.Name, Strings.DataStoreNames.SpellsStore, SerializerSettings);
        internal static DataStore GetUserSpellsStore(ulong UserId)
        {
            DataStore DS = UserSpellsMainDirectory;
            DS.MakeSubStore(UserId.ToString(CultureInfo.InvariantCulture));
            return DS;
        }

        #endregion

        #region relationships

        internal static DataStore UserRelationshipsMainDirectory => new(Strings.App.Name, Strings.DataStoreNames.RelationshipsStore, SerializerSettings);

        #endregion

        #region rate limits
        internal static DataStore RateLimitsStore => new(Strings.App.Name, Strings.DataStoreNames.RateLimitStore, SerializerSettings);
        internal static DataStore SaleListingsStore => new(Strings.App.Name, Strings.DataStoreNames.SaleListingsStore, SerializerSettings);
        #endregion

        #region lofi
        internal static DataStore LoFiUserStore => izolabella.LoFi.Server.Structures.Constants.DataStores.UserStore;
        internal static DataStore LoFiUserListensStore(ulong UserId) => izolabella.LoFi.Server.Structures.Constants.DataStores.GetUsersListensStore(UserId);
        #endregion
    }
}
