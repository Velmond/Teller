namespace Teller.Web.Controllers.Search
{
    using System;
    using System.Collections.Generic;
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

            model.Stories = this.GetPageStories(pattern, pageNumber);
            model.Series = this.GetPageSeries(pattern, pageNumber);
            model.Users = this.GetPageUsers(pattern, pageNumber);

            ViewBag.Pages = this.GetPagesCount(pattern);

            return this.View(model);
        }

        [NonAction]
        private IEnumerable<SearchStoryViewModel> GetPageStories(string pattern, int pageNumber)
        {
            var data = this.HttpContext.Cache[Constants.SearchStoriesCachePrefix + pattern + pageNumber.ToString()] as IEnumerable<SearchStoryViewModel>;

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
                    .OrderBy(s => s.Title)
                    .Skip((pageNumber - 1) * Constants.StoriesPerSearchPage)
                    .Take(Constants.StoriesPerSearchPage)
                    .ToList();

                this.HttpContext.Cache.Add(
                    Constants.SearchStoriesCachePrefix + pattern + pageNumber.ToString(),
                    data,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            return data;
        }

        [NonAction]
        private IEnumerable<SearchSeriesViewModel> GetPageSeries(string pattern, int pageNumber)
        {
            var data = this.HttpContext.Cache[Constants.SearchSeriesCachePrefix + pattern + pageNumber.ToString()] as IEnumerable<SearchSeriesViewModel>;

            if (data == null)
            {
                data = this.Data.Series.All()
                    .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                    .Select(SearchSeriesViewModel.FromSeries)
                    .OrderBy(s => s.Title)
                    .Skip((pageNumber - 1) * Constants.SeriesPerSearchPage)
                    .Take(Constants.SeriesPerSearchPage)
                    .ToList();

                this.HttpContext.Cache.Add(
                    Constants.SearchSeriesCachePrefix + pattern + pageNumber.ToString(),
                    data,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            return data;
        }

        [NonAction]
        private IEnumerable<SearchUserViewModel> GetPageUsers(string pattern, int pageNumber)
        {
            var data = this.HttpContext.Cache[Constants.SearchUsersCachePrefix + pattern + pageNumber.ToString()] as IEnumerable<SearchUserViewModel>;

            if (data == null)
            {
                data = this.Data.Users.All()
                    .Where(u => u.UserName.ToLower().IndexOf(pattern) >= 0)
                    .Select(SearchUserViewModel.FromUser)
                    .OrderBy(u => u.Username)
                    .Skip((pageNumber - 1) * Constants.UsersPerSearchPage)
                    .Take(Constants.UsersPerSearchPage)
                    .ToList();

                this.HttpContext.Cache.Add(
                    Constants.SearchUsersCachePrefix + pattern + pageNumber.ToString(),
                    data,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            return data;
        }

        [NonAction]
        private int GetPagesCount(string pattern)
        {
            int pages;
            var cached = this.HttpContext.Cache["search-pages-count-cache-" + pattern];

            if (cached == null)
            {
                var storiesPageCount = Math.Ceiling((double)this.GetAllStoriesCount(pattern) / Constants.StoriesPerSearchPage);
                var seriesPageCount = Math.Ceiling((double)this.GetAllSeriesCount(pattern) / Constants.SeriesPerSearchPage);
                var usersPageCount = Math.Ceiling((double)this.GetAllUsersCount(pattern) / Constants.UsersPerSearchPage);

                pages = (int)Math.Max(storiesPageCount, Math.Max(seriesPageCount, usersPageCount));

                this.HttpContext.Cache.Add(
                    "search-pages-count-cache-" + pattern,
                    pages,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }
            else
            {
                pages = int.Parse(cached.ToString());
            }

            return pages;
        }

        [NonAction]
        private int GetAllUsersCount(string pattern)
        {
            var count = this.Data.Stories.All()
                            .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                            .Union(this.Data.Stories.All()
                                .Where(s => s.Content.ToLower().IndexOf(pattern) >= 0))
                            .Union(this.Data.Stories.All()
                                .Where(s => s.Series.Title.ToLower().IndexOf(pattern) >= 0))
                            .Union(this.Data.Stories.All()
                                .Where(s => s.Author.UserName.ToLower() == pattern))
                            .Count();
            return count;
        }

        private int GetAllSeriesCount(string pattern)
        {
            var count = this.Data.Series.All()
                            .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                            .Count();
            return count;
        }

        [NonAction]
        private int GetAllStoriesCount(string pattern)
        {
            var count = this.Data.Users.All()
                            .Where(u => u.UserName.ToLower().IndexOf(pattern) >= 0)
                            .Count();
            return count;
        }
    }
}
