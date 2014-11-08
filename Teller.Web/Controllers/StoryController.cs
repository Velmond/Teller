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
    using Teller.Web.Helpers;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Story;

    public class StoryController : BaseController
    {
        public StoryController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string username)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username) || username.IndexOf('-') < 0)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            int idInUrl;
            if(!int.TryParse(username.Substring(username.LastIndexOf('-') + 1), out idInUrl))
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            var foundStory = this.Data.Stories.Find(idInUrl);
            var url = new UrlGenerator();
            var foundStoryId = url.GenerateUrlId(foundStory.Id, foundStory.Title);

            if(foundStoryId != username)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            var story = new StoryCompleteViewModel()
            {
                Id = foundStory.Id,
                Title = foundStory.Title,
                Content = foundStory.Content,
                DatePublished = foundStory.DatePublished,
                PicturePath = foundStory.PicturePath,
                ViewsCount = foundStory.ViewsCount,
                Author = foundStory.Author.UserName,
                Genre = foundStory.Genre.Name,
                LikesCount = foundStory.Likes.Count(l => l.Value == true),
                DislikesCount = foundStory.Likes.Count(l => l.Value == false),
                FavouritedByCount = foundStory.FavouritedBy.Count(),
                IsFlagged = foundStory.Flags.Any(f => !f.IsResolved),
                Comments = foundStory.Comments.Select(CommentViewModel.FromComment),
            };

            if(story.Series != null)
            {
                story.Series = new SeriesViewModel()
                {
                    Id = foundStory.Series.Id,
                    Title = foundStory.Series.Title,
                    Genre = foundStory.Series.Genre.Name,
                    Author = foundStory.Series.Author.UserName
                };
            }




            //var story = this.Data.Stories.All()
            //    .Where(s => s.Id == idInUrl)
            //    .Select(StoryCompleteViewModel.FromStory)
            //    .FirstOrDefault();

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