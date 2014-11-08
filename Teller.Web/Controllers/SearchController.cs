namespace Teller.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.ViewModels.Search;

    public class SearchController : BaseController
    {
        private const int StoriesPageSize = 18;
        private const int SeriesPageSize = 5;
        private const int UsersPageSize = 5;

        public SearchController(ITellerData data)
            : base(data)
        {
        }

        public IQueryable<SearchStoryViewModel> GetAllStories(string pattern)
        {
            var data = this.Data.Stories.All()
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

            return data;
        }

        public IQueryable<SearchSeriesViewModel> GetAllSeries(string pattern)
        {
            var data = this.Data.Series.All()
                .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                .Select(SearchSeriesViewModel.FromSeries)
                .OrderBy(s => s.Title);

            return data;
        }

        public IQueryable<SearchUserViewModel> GetAllUsers(string pattern)
        {
            var data = this.Data.Users.All()
                .Where(u => u.UserName.ToLower().IndexOf(pattern) >= 0)
                .Select(SearchUserViewModel.FromUser)
                .OrderBy(u => u.Username);

            return data;
        }

        public ActionResult Index(string pattern, int? page)
        {
            if(string.IsNullOrEmpty(pattern) || string.IsNullOrWhiteSpace(pattern))
            {
                return RedirectToAction("Index", "Home");
            }

            var pageNumber = page.GetValueOrDefault(1);
            ViewBag.Page = pageNumber;

            var model = new SearchViewModel();
            model.Pattern = pattern;

            pattern = pattern.ToLower();

            model.Stories = this.GetAllStories(pattern)
                .Skip((pageNumber - 1) * StoriesPageSize)
                .Take(StoriesPageSize)
                .ToList();

            model.Series = this.GetAllSeries(pattern)
                .Skip((pageNumber - 1) * SeriesPageSize)
                .Take(SeriesPageSize)
                .ToList();

            model.Users = this.GetAllUsers(pattern)
                .Skip((pageNumber - 1) * UsersPageSize)
                .Take(UsersPageSize)
                .ToList();

            var storiesPageCount = Math.Ceiling((double)this.GetAllStories(pattern).Count() / StoriesPageSize);
            var seriesPageCount = Math.Ceiling((double)this.GetAllSeries(pattern).Count() / SeriesPageSize);
            var usersPageCount = Math.Ceiling((double)this.GetAllUsers(pattern).Count() / UsersPageSize);
            ViewBag.Pages = Math.Max(storiesPageCount, Math.Max(seriesPageCount, usersPageCount));

            return View(model);
        }
    }
}