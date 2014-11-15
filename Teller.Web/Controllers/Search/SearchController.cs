namespace Teller.Web.Controllers.Search
{
    using System;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Teller.Data.UnitsOfWork;
    using Teller.Web.Controllers.Base;
    using Teller.Web.Helpers;
    using Teller.Web.ViewModels.Search;

    public class SearchController : BaseController
    {
        public SearchController(ITellerData data)
            : base(data)
        {
        }

        public IQueryable<SearchStoryViewModel> GetAllStories(string pattern)
        {
            IQueryable<SearchStoryViewModel> data = this.HttpContext.Cache[ProjectConstants.SearchStoriesCachePrefix + pattern] as IQueryable<SearchStoryViewModel>;

            if (data == null)
            {
                data = this.Data.Stories.All()
                    .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                    .Select(SearchStoryViewModel.FromStory)
                    .Union(this.Data.Stories.All()
                        .Where(s => s.Content.ToLower().IndexOf(pattern) >= 0)
                        .Select(SearchStoryViewModel.FromStory))
                    .Union(this.Data.Stories.All()
                        .Where(s => s.Series.Title.ToLower().IndexOf(pattern) >= 0)
                        .Select(SearchStoryViewModel.FromStory))
                    .Union(this.Data.Stories.All()
                        .Where(s => s.Author.UserName.ToLower() == pattern)
                        .Select(SearchStoryViewModel.FromStory))
                    .OrderBy(s => s.Title);

                this.HttpContext.Cache.Add(
                    ProjectConstants.SearchStoriesCachePrefix + pattern,
                    data,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            return data;
        }

        public IQueryable<SearchSeriesViewModel> GetAllSeries(string pattern)
        {
            IQueryable<SearchSeriesViewModel> data = this.HttpContext.Cache[ProjectConstants.SearchSeriesCachePrefix + pattern] as IQueryable<SearchSeriesViewModel>;

            if (data == null)
            {
                data = this.Data.Series.All()
                    .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                    .Select(SearchSeriesViewModel.FromSeries)
                    .OrderBy(s => s.Title);

                this.HttpContext.Cache.Add(
                    ProjectConstants.SearchSeriesCachePrefix + pattern,
                    data,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            return data;
        }

        public IQueryable<SearchUserViewModel> GetAllUsers(string pattern)
        {
            IQueryable<SearchUserViewModel> data = this.HttpContext.Cache[ProjectConstants.SearchUsersCachePrefix + pattern] as IQueryable<SearchUserViewModel>;

            if (data == null)
            {
                data = this.Data.Users.All()
                    .Where(u => u.UserName.ToLower().IndexOf(pattern) >= 0)
                    .Select(SearchUserViewModel.FromUser)
                    .OrderBy(u => u.Username);

                this.HttpContext.Cache.Add(
                    ProjectConstants.SearchUsersCachePrefix + pattern,
                    data,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            return data;
        }

        [ValidateInput(false)]
        public ActionResult Index(string pattern, int? page)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrWhiteSpace(pattern))
            {
                return this.RedirectToAction("Index", "Home");
            }

            var pageNumber = page.GetValueOrDefault(1);
            ViewBag.Page = pageNumber;

            var model = new SearchViewModel();
            model.Pattern = pattern;

            pattern = pattern.ToLower();

            model.Stories = this.GetAllStories(pattern)
                .Skip((pageNumber - 1) * ProjectConstants.StoriesPerSearchPage)
                .Take(ProjectConstants.StoriesPerSearchPage)
                .ToList();

            model.Series = this.GetAllSeries(pattern)
                .Skip((pageNumber - 1) * ProjectConstants.SeriesPerSearchPage)
                .Take(ProjectConstants.SeriesPerSearchPage)
                .ToList();

            model.Users = this.GetAllUsers(pattern)
                .Skip((pageNumber - 1) * ProjectConstants.UsersPerSearchPage)
                .Take(ProjectConstants.UsersPerSearchPage)
                .ToList();

            var storiesPageCount = Math.Ceiling((double)this.GetAllStories(pattern).Count() / ProjectConstants.StoriesPerSearchPage);
            var seriesPageCount = Math.Ceiling((double)this.GetAllSeries(pattern).Count() / ProjectConstants.SeriesPerSearchPage);
            var usersPageCount = Math.Ceiling((double)this.GetAllUsers(pattern).Count() / ProjectConstants.UsersPerSearchPage);
            ViewBag.Pages = Math.Max(storiesPageCount, Math.Max(seriesPageCount, usersPageCount));

            return this.View(model);
        }
    }
}
