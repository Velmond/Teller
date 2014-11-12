namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.ViewModels;

    public class FeedController : BaseController
    {
        private const int PageSize = 10;
        ////private const string FeedCacheKey = "subscriptions";
        ////private const string FavoritesCacheKey = "favorites";
        ////private const string ReadLaterCacheKey = "read-later";

        public FeedController(ITellerData data)
            : base(data)
        {
        }

        [Authorize]
        public ActionResult Index(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);
            var collections = this.User.SubscribedTo.Select(s => s.Stories);

            List<UserFeedStory> stories = new List<UserFeedStory>();

            foreach (var collection in collections)
            {
                stories.AddRange(collection.AsQueryable().Select(UserFeedStory.FromStory));
            }

            IEnumerable<UserFeedStory> data = stories.OrderByDescending(s => s.DatePublished);

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)data.Count() / PageSize);

            var model = data.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            return this.View(model);
        }

        [Authorize]
        public ActionResult Favorites(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var stories = this.User.Favourites
                .AsQueryable()
                    .Select(UserFeedStory.FromStory)
                    .OrderByDescending(s => s.DatePublished);

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)stories.Count() / PageSize);

            var model = stories.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            return this.View(model);
        }

        [Authorize]
        public ActionResult ReadLater(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            IEnumerable<UserFeedStory> stories = this.User.ReadLater
                .AsQueryable()
                .Select(UserFeedStory.FromStory)
                .OrderByDescending(s => s.DatePublished);

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)stories.Count() / PageSize);

            var model = stories.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            return this.View(model);
        }

        private void CacheValues(IEnumerable<UserFeedStory> stories, string cacheKey)
        {
            this.HttpContext.Cache.Add(
                cacheKey,
                stories,
                null,
                DateTime.Now.AddMinutes(15),
                Cache.NoSlidingExpiration,
                CacheItemPriority.AboveNormal,
                null);
        }
    }
}
