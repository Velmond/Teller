namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Story;

    public class StoryController : BaseController
    {
        public StoryController(ITellerData data)
            : base(data)
        {
        }

        // ~/story/{id}
        public ActionResult Index(string id)
        {
            // get story with id = {id}
            var storyId = id.Substring(id.LastIndexOf('-') + 1);
            var story = this.Data.Stories.Find(storyId);

            return View(story);
        }

        [Authorize]
        public ActionResult Create()
        {
            var model = new StoryFormViewModel();

            model.GenresList = new SelectViewModel()
            {
                List = this.Data.Genres.All()
                    .Select(g => new SelectListItem()
                        {
                            Value = g.Id.ToString(),
                            Text = g.Name
                        })
            };

            model.SeriesList = new SelectViewModel()
            {
                List = this.Data.Series.All()
                    .Where(s => s.AuthorId == this.User.Id)
                    .Select(g => new SelectListItem()
                        {
                            Value = g.Id.ToString(),
                            Text = g.Title
                        })
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoryFormViewModel story)
        {
            var newStory = new Story()
            {
                AuthorId = this.User.Id,
                Author = this.User,
                Content = story.Content,
                DatePublished = DateTime.Now,
                GenreId = story.GenreId,
                Genre = this.Data.Genres.Find(story.GenreId),
                PicturePath = @"~\Images\StoryPictures\default\cover.jpg",
                SeriesId = story.SeriesId == null ? null : story.SeriesId,
                Series = story.SeriesId == null ? null : this.Data.Series.Find(story.SeriesId),
                Title = story.Title,
            };
            this.Data.Stories.Add(newStory);

            this.Data.Stories.SaveChanges();
            return RedirectToAction("Index", new { id = string.Join("-", newStory.Title.ToLower().Substring(0, Math.Min(20, newStory.Title.Length)).Split(new char[] { ' ', '.', '-', ',', ':', ';', '?', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '[', ']', '{', '}', '<', '>', '\\', '/', '|', '\'', '"' }, StringSplitOptions.RemoveEmptyEntries)) + "-" + newStory.Id });
        }
    }
}