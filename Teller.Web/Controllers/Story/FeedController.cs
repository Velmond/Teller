namespace Teller.Web.Controllers.Story
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;
    using Teller.Data.UnitsOfWork;
    using Teller.Web.Controllers.Base;
    using Teller.Web.Helpers;
    using Teller.Web.ViewModels.Story;

    public class FeedController : BaseController
    {
        public FeedController(ITellerData data)
            : base(data)
        {
        }

        [Authorize]
        public ActionResult Index(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);
            var collections = this.UserProfile.SubscribedTo.Select(s => s.Stories);

            List<UserFeedStory> stories = new List<UserFeedStory>();

            foreach (var collection in collections)
            {
                stories.AddRange(collection.AsQueryable().Select(UserFeedStory.FromStory));
            }

            IEnumerable<UserFeedStory> data = stories.OrderByDescending(s => s.DatePublished);

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)data.Count() / ProjectConstants.StoriesPerFeedPage);

            var model = data.Skip((pageNumber - 1) * ProjectConstants.StoriesPerFeedPage)
                .Take(ProjectConstants.StoriesPerFeedPage)
                .ToList();

            return this.View(model);
        }

        [Authorize]
        public ActionResult Favorites(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var stories = this.UserProfile.Favourites
                .AsQueryable()
                    .Select(UserFeedStory.FromStory)
                    .OrderByDescending(s => s.DatePublished);

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)stories.Count() / ProjectConstants.StoriesPerFeedPage);

            var model = stories.Skip((pageNumber - 1) * ProjectConstants.StoriesPerFeedPage)
                .Take(ProjectConstants.StoriesPerFeedPage)
                .ToList();

            return this.View(model);
        }

        [Authorize]
        public ActionResult ReadLater(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            IEnumerable<UserFeedStory> stories = this.UserProfile.ReadLater
                .AsQueryable()
                .Select(UserFeedStory.FromStory)
                .OrderByDescending(s => s.DatePublished);

            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)stories.Count() / ProjectConstants.StoriesPerFeedPage);

            var model = stories.Skip((pageNumber - 1) * ProjectConstants.StoriesPerFeedPage)
                .Take(ProjectConstants.StoriesPerFeedPage)
                .ToList();

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
