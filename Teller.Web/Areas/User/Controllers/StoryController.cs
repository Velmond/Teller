namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;
    using Teller.Data.UnitsOfWork;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Controllers.Base;
    using Teller.Web.Helpers;
    using Teller.Web.ViewModels.Story;

    public class StoryController : BaseController
    {
        public StoryController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var user = this.Data.Users.All()
                .Select(StoriesViewModel.FromUser)
                .SingleOrDefault(u => u.Username == id);

            if (user == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var userStories = this.HttpContext.Cache[ProjectConstants.UserStoriesCacheKeyPrefix + id] as IQueryable<UserFeedStory>;

            if (userStories == null)
            {
                userStories = this.Data.Stories.All()
                    .Where(s => s.Author.UserName == id)
                    .OrderByDescending(s => s.DatePublished)
                    .Select(UserFeedStory.FromStory);

                this.HttpContext.Cache.Add(
                    ProjectConstants.UserStoriesCacheKeyPrefix + id,
                    userStories,
                    null,
                    DateTime.Now.AddMinutes(15),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);
            }

            if (this.UserProfile != null)
            {
                ViewBag.IsSubscribedTo = this.UserProfile.SubscribedTo.Any(u => u.UserName == id);
            }

            ViewBag.Username = id;
            ViewBag.AvatarPath = user.AvatarPath;
            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)userStories.Count() / ProjectConstants.UserProfilePageSize);

            user.Stories = userStories.Skip((pageNumber - 1) * ProjectConstants.UserProfilePageSize).Take(ProjectConstants.UserProfilePageSize);

            return this.View(user);
        }
    }
}