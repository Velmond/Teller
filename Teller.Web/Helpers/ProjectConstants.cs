namespace Teller.Web.Helpers
{
    public static class ProjectConstants
    {
        public const int SeriesPerSearchPage = 6;
        public const int UsersPerSearchPage = 8;
        public const int StoriesPerSearchPage = 9;
        public const int UserProfilePageSize = 10;
        public const int StoriesPerFeedPage = 12;
        public const int PageSize16 = 16;

        public const string HomeStoriesCacheName = "home-page-stories-cache";
        public const string SearchStoriesCachePrefix = "search-stories-cache-";
        public const string SearchSeriesCachePrefix = "search-series-cache-";
        public const string SearchUsersCachePrefix = "search-users-cache-";
        public const string FeedCacheKeyPrefix = "subscriptions-feed-";
        public const string FavoritesCacheKeyPrefix = "favorites-list-";
        public const string ReadLaterCacheKeyPrefix = "read-later-list-";
        public const string UserStoriesCacheKeyPrefix = "user-profile-stories-";
        public const string UserSeriesCacheKeyPrefix = "user-profile-series-";
        public const string UserRolesCacheKey = "admin-panel-user-roles";
        public const string GenresCacheKey = "admin-panel-genres";
    }
}
