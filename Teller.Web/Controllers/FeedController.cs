namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.ViewModels;

    public class FeedController : BaseController
    {
        private const int PageSize = 4;
        private const string FeedCacheKey = "feed";
        private const string FavoritesCacheKey = "favorites";
        private const string ReadLaterCacheKey = "read-later";

        public FeedController(ITellerData data)
            : base(data)
        {
        }

        private IQueryable<UserFeedStory> GetAllFeedStories(string cacheKey)
        {
            var data = this.HttpContext.Cache[cacheKey + "-stories-cache-" + this.User.UserName];

            if(data == null)
            {
                IEnumerable<ICollection<Story>> collections;

                switch(cacheKey)
                {
                    case FavoritesCacheKey:
                        data = this.User.Favourites.Select(UserFeedStory.FromStory);
                        break;
                    case ReadLaterCacheKey:
                        data = this.User.ReadLater.Select(UserFeedStory.FromStory);
                        break;
                    case FeedCacheKey:
                    default:
                        collections = this.User.SubscribedTo.Select(s => s.Stories);
                        foreach(var collection in collections)
                        {
                            (data as List<UserFeedStory>).AddRange(collection.Select(UserFeedStory.FromStory));
                        }
                        break;
                }

                this.HttpContext.Cache.Add(
                    cacheKey + "-stories-cache-" + this.User.UserName,
                    data,
                    null,
                    DateTime.Now.AddMinutes(15),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            return (data as IQueryable<UserFeedStory>);
        }

        [Authorize]
        public ActionResult Index(string username, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);
            IEnumerable<UserFeedStory> data = this.HttpContext.Cache[FeedCacheKey + "-" + username + "-" + pageNumber] as IEnumerable<UserFeedStory>;
            
            if(data == null)
            {
                var collections = this.User.SubscribedTo.Select(s => s.Stories);
                List<UserFeedStory> stories = new List<UserFeedStory>();

                foreach(var collection in collections)
                {
                    stories.AddRange(collection.Select(UserFeedStory.FromStory));
                }

                data = stories.OrderByDescending(s => s.DatePublished);
                this.CacheValues(data, FeedCacheKey, username, pageNumber);
            }

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)data.Count() / PageSize);

            var model = data.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult Favorites(string username, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);
            IEnumerable<UserFeedStory> stories = this.HttpContext.Cache[FavoritesCacheKey + "-" + username + "-" + pageNumber] as IEnumerable<UserFeedStory>;

            if(stories == null)
            {
                stories = this.User.Favourites
                    .Select(UserFeedStory.FromStory)
                    .OrderByDescending(s => s.DatePublished);

                this.CacheValues(stories, FavoritesCacheKey, username, pageNumber);
            }

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)stories.Count() / PageSize);

            var model = stories.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult ReadLater(string username, int? page)
        {

            var pageNumber = page.GetValueOrDefault(1);
            IEnumerable<UserFeedStory> stories = this.HttpContext.Cache[ReadLaterCacheKey + "-" + username + "-" + pageNumber] as IEnumerable<UserFeedStory>;

            if(stories == null)
            {
                stories = this.User.ReadLater
                    .Select(UserFeedStory.FromStory)
                    .OrderByDescending(s => s.DatePublished);

                this.CacheValues(stories, ReadLaterCacheKey, username, pageNumber);
            }

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)stories.Count() / PageSize);

            var model = stories.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            return View(model);
        }

        private void CacheValues(IEnumerable<UserFeedStory> stories, string cacheKey, string username, int pageNumber)
        {
            this.HttpContext.Cache.Add(
                cacheKey + "-" + username + "-" + pageNumber,
                stories,
                null,
                DateTime.Now.AddMinutes(15),
                Cache.NoSlidingExpiration,
                CacheItemPriority.AboveNormal,
                null);
        }
    }
}
