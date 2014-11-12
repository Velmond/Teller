namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Search;

    public class HomeController : BaseController
    {
        public HomeController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var cachedCollections = this.HttpContext.Cache["HomeStories"];
            if (cachedCollections == null)
            {
                var topHundredEver = this.GetMostVotedStoriesEver();
                var topHundredLastWeek = this.GetMostVotedThisWeek();
                var topHundredCommentedEver = this.GetMostCommentedEver();
                var topHundredCommentedLastWeek = this.GetMostCommentedLastWeek();

                cachedCollections = new HomePageViewModel()
                {
                    MostVotedStoriesEver = topHundredEver,
                    MostVotedStoriesLastWeek = topHundredLastWeek,
                    MostCommentedStoriesEver = topHundredCommentedEver,
                    MostCommentedStoriesLastWeek = topHundredCommentedLastWeek
                };

                this.HttpContext.Cache.Add(
                    "HomeStories",
                    cachedCollections,
                    null,
                    DateTime.Now.AddHours(12),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }

            var storiesToReturn = GetItemsToDisplay(cachedCollections);

            return this.View(storiesToReturn);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return this.View();
        }

        [NonAction]
        private static HomePageViewModel GetItemsToDisplay(object cachedCollections)
        {
            var nineFromTopEver =
                (cachedCollections as HomePageViewModel)
                    .MostVotedStoriesEver
                    .OrderBy(s => Guid.NewGuid())
                    .Take(9);

            var nineFromTopLastWeek =
                (cachedCollections as HomePageViewModel)
                    .MostVotedStoriesLastWeek
                    .OrderBy(s => Guid.NewGuid())
                    .Take(9);

            var nineFromTopCommentedEver =
                (cachedCollections as HomePageViewModel)
                    .MostCommentedStoriesEver
                    .OrderBy(s => Guid.NewGuid())
                    .Take(9);

            var nineFromTopCommentedLastWeek =
                (cachedCollections as HomePageViewModel)
                    .MostCommentedStoriesLastWeek
                    .OrderBy(s => Guid.NewGuid())
                    .Take(9);

            return new HomePageViewModel()
            {
                MostVotedStoriesEver = nineFromTopEver,
                MostVotedStoriesLastWeek = nineFromTopLastWeek,
                MostCommentedStoriesEver = nineFromTopCommentedEver,
                MostCommentedStoriesLastWeek = nineFromTopCommentedLastWeek
            };
        }

        [NonAction]
        private IEnumerable<SearchStoryViewModel> GetMostCommentedLastWeek()
        {
            var dateLastWeek = DateTime.Now.AddDays(-7);

            var topHundredLastWeek = this.Data.Stories.All()
                .Where(s => s.DatePublished > dateLastWeek)
                .OrderByDescending(s => s.Comments.Count())
                .Select(SearchStoryViewModel.FromStory)
                .Take(100)
                .ToList();

            return topHundredLastWeek;
        }

        [NonAction]
        private IEnumerable<SearchStoryViewModel> GetMostCommentedEver()
        {
            var topHundredEver = this.Data.Stories.All()
                .OrderByDescending(s => s.Comments.Count())
                .Select(SearchStoryViewModel.FromStory)
                .Take(100)
                .ToList();

            return topHundredEver;
        }

        [NonAction]
        private IEnumerable<SearchStoryViewModel> GetMostVotedThisWeek()
        {
            var dateLastWeek = DateTime.Now.AddDays(-7);

            var topHundredLastWeek = this.Data.Stories.All()
                .Where(s => s.DatePublished > dateLastWeek)
                .OrderByDescending(s => s.Likes.Count())
                .Select(SearchStoryViewModel.FromStory)
                .Take(100)
                .ToList();

            return topHundredLastWeek;
        }

        [NonAction]
        private IEnumerable<SearchStoryViewModel> GetMostVotedStoriesEver()
        {
            var topHundredEver = this.Data.Stories.All()
                .OrderByDescending(s => s.Likes.Count())
                .Select(SearchStoryViewModel.FromStory)
                .Take(100)
                .ToList();

            return topHundredEver;
        }
    }
}