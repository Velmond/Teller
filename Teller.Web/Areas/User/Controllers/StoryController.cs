namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Controllers;
    using Teller.Web.ViewModels;

    public class StoryController : BaseController
    {
        private const int PageSize = 10;

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
            
            if(user == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            var userStories = this.HttpContext.Cache["user-profile-stories-" + id] as IEnumerable<UserFeedStory>;
            if(userStories== null)
            {
                userStories =this.Data.Stories.All()
                    .Where(s => s.AuthorId == this.User.Id)
                    .Select(UserFeedStory.FromStory)
                    .OrderByDescending(s => s.DatePublished);

                this.HttpContext.Cache.Add(
                    "user-profile-stories-" + id,
                    userStories,
                    null,
                    DateTime.Now.AddMinutes(15),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);
            }

            ViewBag.Username = id;
            ViewBag.AvatarPath = user.AvatarPath;
            ViewBag.Page = pageNumber;
            ViewBag.Pages = Math.Ceiling((double)userStories.Count() / PageSize);

            user.Stories = userStories.Skip((pageNumber - 1) * PageSize).Take(PageSize);

            return View(user);
        }
    }
}