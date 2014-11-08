namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Web.ViewModels.Search;

    public class SearchController : BaseController
    {
        public SearchController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string pattern)
        {
            if(string.IsNullOrEmpty(pattern))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Pattern = pattern;
            pattern = pattern.ToLower();

            var model = new SearchViewModel();
            
            model.Pattern = pattern;

            var storiresByContent = this.Data.Stories.All()
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
                    .Select(SearchStoryViewModel.FromStory));

            model.Stories = storiresByContent.ToList();
            model.Series = this.Data.Series.All()
                .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                .Select(SearchSeriesViewModel.FromSeries)
                .ToList();

            model.Users = this.Data.Users.All()
                .Where(u => u.UserName.ToLower().IndexOf(pattern) >= 0)
                .Select(SearchUserViewModel.FromUser)
                .ToList();

            return View(model);
        }

        public ActionResult QueryStories(string pattern)
        {
            pattern = pattern.ToLower();

            var model = new SearchViewModel();

            model.Pattern = pattern;

            var storiresByContent = this.Data.Stories.All()
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
                    .Select(SearchStoryViewModel.FromStory));

            model.Stories = storiresByContent.ToList();
            model.Series = null;
            model.Users = null;

            return PartialView("_StorySummaryPartial", model.Stories);
        }

        public ActionResult QuerySeries(string pattern)
        {
            pattern = pattern.ToLower();

            var model = new SearchViewModel();

            model.Pattern = pattern;

            model.Series = this.Data.Series.All()
                .Where(s => s.Title.ToLower().IndexOf(pattern) >= 0)
                .Select(SearchSeriesViewModel.FromSeries)
                .ToList();

            model.Stories = null;
            model.Users = null;

            return PartialView("_SearchSeriesPartial", model.Series);
        }

        public ActionResult QueryUsers(string pattern)
        {
            pattern = pattern.ToLower();

            var model = new SearchViewModel();

            model.Pattern = pattern;

            model.Users = this.Data.Users.All()
                .Where(u => u.UserName.ToLower().IndexOf(pattern) >= 0)
                .Select(SearchUserViewModel.FromUser)
                .ToList();

            model.Stories = null;
            model.Series = null;

            return PartialView("_SearchUsersPartial", model.Users);
        }
    }
}