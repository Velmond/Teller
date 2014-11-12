namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Controllers;
    using Teller.Web.ViewModels.Series;

    public class SeriesController : BaseController
    {
        private const int PageSize = 10;
        private const string SeriesCacheKeyPrefix = "user-profile-series-";

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

            var userSeries = this.HttpContext.Cache[SeriesCacheKeyPrefix + id] as IQueryable<SeriesViewModel>;

            if (userSeries == null)
            {
                userSeries = this.Data.Series.All()
                    .Where(s => s.AuthorId == user.Id)
                    .Where(s => s.Stories.Count() > 0)
                    .OrderBy(s => s.Title)
                    .Select(SeriesViewModel.FromSeries);

                this.HttpContext.Cache.Add(
                    SeriesCacheKeyPrefix + id,
                    userSeries,
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
            ViewBag.Pages = Math.Ceiling((double)userSeries.Count() / PageSize);

            user.Series = userSeries.Skip((pageNumber - 1) * PageSize).Take(PageSize);

            return this.View(user);
        }
    }
}