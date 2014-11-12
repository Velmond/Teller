namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Controllers;
    using Teller.Web.ViewModels;

    public class StoryController : BaseController
    {
        private const int PageSize = 10;
        private const string StoriesCacheKeyPrefix = "user-profile-stories-";

        public StoryController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var user = this.Data.Users.All()
                .Select(UserStoriesViewModel.FromUser)
                .SingleOrDefault(u => u.Username == id);

            if (user == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var userStories = this.HttpContext.Cache[StoriesCacheKeyPrefix + id] as IQueryable<UserFeedStory>;

            if (userStories == null)
            {
                userStories = this.Data.Stories.All()
                    .Where(s => s.Author.UserName == id)
                    .OrderByDescending(s => s.DatePublished)
                    .Select(UserFeedStory.FromStory);

                this.HttpContext.Cache.Add(
                    StoriesCacheKeyPrefix + id,
                    userStories,
                    null,
                    DateTime.Now.AddMinutes(15),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);
            }

            if (this.User != null)
            {
                ViewBag.IsSubscribedTo = this.User.SubscribedTo.Any(u => u.UserName == id);
            }

            ViewBag.Username = id;
            ViewBag.AvatarPath = user.AvatarPath;
            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)userStories.Count() / PageSize);

            user.Stories = userStories.Skip((pageNumber - 1) * PageSize).Take(PageSize);

            return this.View(user);
        }
    }
}