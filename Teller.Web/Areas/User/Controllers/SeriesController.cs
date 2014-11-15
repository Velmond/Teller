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
    using Teller.Web.ViewModels.Series;

    public class SeriesController : BaseController
    {
        public SeriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var user = this.Data.Users.All()
                .Select(UserSeriesViewModel.FromUser)
                .SingleOrDefault(u => u.Username == id);

            if (user == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var userSeries = this.HttpContext.Cache[ProjectConstants.UserSeriesCacheKeyPrefix + id] as IQueryable<SeriesViewModel>;

            if (userSeries == null)
            {
                userSeries = this.Data.Series.All()
                    .Where(s => s.AuthorId == user.Id)
                    .Where(s => s.Stories.Count() > 0)
                    .OrderBy(s => s.Title)
                    .Select(SeriesViewModel.FromSeries);

                this.HttpContext.Cache.Add(
                    ProjectConstants.UserSeriesCacheKeyPrefix + id,
                    userSeries,
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
            ViewBag.Pages = Math.Ceiling((double)userSeries.Count() / ProjectConstants.UserProfilePageSize);

            user.Series = userSeries.Skip((pageNumber - 1) * ProjectConstants.UserProfilePageSize).Take(ProjectConstants.UserProfilePageSize);

            return this.View(user);
        }
    }
}