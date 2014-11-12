namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.Infrastructure;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Series;
    using Teller.Web.ViewModels.Story;

    public class SeriesController : BaseController
    {
        private const string SeriesCreateFormPartialName = "_SeriesCreateFormPartial";

        public SeriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int seriesId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out seriesId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var foundSeries = this.Data.Series.Find(seriesId);

            if (foundSeries == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedSeriesId = url.GenerateUrlId(foundSeries.Id, foundSeries.Title);

            if (encodedSeriesId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var series = new SeriesViewModel()
            {
                Id = foundSeries.Id,
                Title = foundSeries.Title,
                Author = foundSeries.Author.UserName,
                Genre = foundSeries.Genre.Name,
                TotalViewsCount = foundSeries.Stories.Sum(s => s.ViewsCount),
                TotalLikesCount = foundSeries.Stories.Sum(s => s.Likes.Count()),
                TotalFavoritesCount = foundSeries.Stories.Sum(s => s.FavouritedBy.Count()),
                Stories = new List<StoryViewModel>()
            };

            foreach (var story in foundSeries.Stories)
            {
                series.Stories.Add(new StoryViewModel()
                {
                    Id = story.Id,
                    Author = story.Author.UserName,
                    Title = story.Title,
                    Content = story.Content.Length > 100 ? story.Content.Substring(0, 100) + "..." : story.Content,
                    DatePublished = story.DatePublished,
                    PicturePath = story.PicturePath
                });
            }

            return this.View(series);
        }

        [Authorize]
        public ActionResult Create()
        {
            var model = new SeriesCreateViewModel();

            model.GenresList = new SelectViewModel()
            {
                List = this.Data.Genres.All()
                    .Select(g => new SelectListItem()
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
            };

            return this.PartialView(SeriesCreateFormPartialName, model);
        }
    }
}