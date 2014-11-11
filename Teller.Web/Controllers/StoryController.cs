namespace Teller.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Helpers;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Series;
    using Teller.Web.ViewModels.Story;

    public class StoryController : BaseController
    {
        public StoryController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            int storyId;
            if(!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            var foundStory = this.Data.Stories.Find(storyId);

            if(foundStory == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(foundStory.Id, foundStory.Title);

            if(encodedStoryId != id)
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
                Author = foundStory.Author.UserName,
                Genre = foundStory.Genre.Name,
                LikesCount = foundStory.Likes.Count(l => l.Value == true),
                DislikesCount = foundStory.Likes.Count(l => l.Value == false),
                FavouritedByCount = foundStory.FavouritedBy.Count(),
                IsFlagged = foundStory.Flags.Any(f => !f.IsResolved),
                UserHasLiked = this.User != null ? foundStory.Likes.Any(l => l.AuthorId == this.User.Id) : true,
                Comments = foundStory.Comments
                    .Select(CommentViewModel.FromComment)
                    .OrderByDescending(c => c.Published)
            };

            if(foundStory.Series != null)
            {
                story.Series = new SeriesViewModel()
                {
                    Id = foundStory.Series.Id,
                    Title = foundStory.Series.Title,
                    Genre = foundStory.Series.Genre.Name,
                    Author = foundStory.Series.Author.UserName
                };
            }

            foundStory.ViewsCount += 1;
            this.Data.SaveChanges();
            story.ViewsCount = foundStory.ViewsCount;

            return View(story);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(this.GetStoryFormModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoryFormViewModel story)
        {
            if(Regex.Matches(story.Content, "<[^>]*script").Count > 0)
            {
                ModelState.AddModelError("Content", "Content cannot have script tags in it.");
            }

            if(!string.IsNullOrEmpty(story.SeriesName) &&
                !string.IsNullOrWhiteSpace(story.SeriesName))
            {
                if(story.SeriesName.Length < 2)
                {
                    ModelState.AddModelError("SeriesName", "Series name must be at least 2 characters long.");
                }
                else if(!story.SeriesGenreId.HasValue)
                {
                    ModelState.AddModelError("SeriesGenreId", "New series must have genre selected.");
                }
            }

            if(ModelState.IsValid)
            {
                Story newStory = new Story()
                {
                    Title = story.Title,
                    AuthorId = this.User.Id,
                    Content = story.Content,
                    DatePublished = DateTime.Now,
                    GenreId = story.GenreId
                };

                newStory.SeriesId = this.GetSeriesId(story);
                newStory.PicturePath = this.GetPicturePath(story.Picture, story.Title);

                this.Data.Stories.Add(newStory);
                this.Data.SaveChanges();

                var url = new UrlGenerator();
                return RedirectToAction("Index", new { id = url.GenerateUrlId(newStory.Id, newStory.Title) });
            }

            var model = this.GetStoryFormModel();
            model.Title = string.IsNullOrEmpty(story.Title) ? "" : story.Title;
            model.Content = string.IsNullOrEmpty(story.Content) ? "" : story.Title;
            model.Picture = story.Picture;
            model.GenreId = story.GenreId;
            model.SeriesId = story.GenreId;
            model.SeriesGenreId = story.SeriesGenreId;
            model.SeriesName = string.IsNullOrEmpty(story.SeriesName) ? "" : story.Title;

            return View(model);
        }

        [Authorize]
        public ActionResult Edit()
        {
            return View(this.GetStoryFormModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StoryFormViewModel story)
        {
            if(Regex.Matches(story.Content, "<[^>]*script").Count > 0)
            {
                ModelState.AddModelError("Content", "Content cannot have script tags in it.");
            }

            if(!string.IsNullOrEmpty(story.SeriesName) &&
                !string.IsNullOrWhiteSpace(story.SeriesName))
            {
                if(story.SeriesName.Length < 2)
                {
                    ModelState.AddModelError("SeriesName", "Series name must be at least 2 characters long.");
                }
                else if(!story.SeriesGenreId.HasValue)
                {
                    ModelState.AddModelError("SeriesGenreId", "New series must have genre selected.");
                }
            }

            if(ModelState.IsValid)
            {
                Story newStory = new Story()
                {
                    Title = story.Title,
                    AuthorId = this.User.Id,
                    Content = story.Content,
                    DatePublished = DateTime.Now,
                    GenreId = story.GenreId
                };

                newStory.SeriesId = this.GetSeriesId(story);
                newStory.PicturePath = this.GetPicturePath(story.Picture, story.Title);

                this.Data.Stories.Add(newStory);
                this.Data.SaveChanges();

                var url = new UrlGenerator();
                return RedirectToAction("Index", new { id = url.GenerateUrlId(newStory.Id, newStory.Title) });
            }

            var model = this.GetStoryFormModel();
            model.Title = string.IsNullOrEmpty(story.Title) ? "" : story.Title;
            model.Content = string.IsNullOrEmpty(story.Content) ? "" : story.Title;
            model.Picture = story.Picture;
            model.GenreId = story.GenreId;
            model.SeriesId = story.GenreId;
            model.SeriesGenreId = story.SeriesGenreId;
            model.SeriesName = string.IsNullOrEmpty(story.SeriesName) ? "" : story.Title;

            return View(model);
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            int storyId;
            if(!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            var story = this.Data.Stories.Find(storyId);

            if(story == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if(encodedStoryId != id)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            this.Data.Stories.Delete(story);
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Flag(string id)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            int storyId;
            if(!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            var story = this.Data.Stories.Find(storyId);

            if(story == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if(encodedStoryId != id)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            story.Flags.Add(new Flag()
            {
                DateFlagged = DateTime.Now,
                FlagType = FlagType.ToBeRemoved,
                IsResolved = false,
                StoryId = storyId,
                UserId = this.User.Id
            });

            this.Data.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Story was successfully flagged");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Favorite(string id)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            int storyId;
            if(!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            var story = this.Data.Stories.Find(storyId);

            if(story == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if(encodedStoryId != id)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            if(this.User.Favourites.Any(s => s == story))
            {
                this.User.Favourites.Remove(story);
                this.Data.SaveChanges();
            }
            else
            {
                this.User.Favourites.Add(story);
                this.Data.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ReadLater(string id)
        {

            if(string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            int storyId;
            if(!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            var story = this.Data.Stories.Find(storyId);

            if(story == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if(encodedStoryId != id)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            if(this.User.ReadLater.Any(s => s == story))
            {
                this.User.ReadLater.Remove(story);
                this.Data.SaveChanges();
            }
            else
            {
                this.User.ReadLater.Add(story);
                this.Data.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private StoryFormViewModel GetStoryFormModel()
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

            return model;
        }

        [NonAction]
        private int? GetSeriesId(StoryCreateViewModel story)
        {
            if(story.SeriesId != null)
            {
                return story.SeriesId;
            }
            else if(story.SeriesName != null && story.SeriesGenreId.HasValue)
            {
                var seriesGenreId = story.SeriesGenreId.Value;

                var series = new Series()
                {
                    Title = story.SeriesName,
                    GenreId = seriesGenreId,
                    AuthorId = this.User.Id
                };

                this.Data.Series.Add(series);
                this.Data.SaveChanges();

                return series.Id;
            }
            else
            {
                return null;
            }
        }

        [NonAction]
        private string GetPicturePath(HttpPostedFileBase httpPostedFileBase, string storyTitle)
        {
            if(httpPostedFileBase != null && (httpPostedFileBase.ContentType.StartsWith("image/")))
            {
                var url = new UrlGenerator();

                string folderPath = string.Format("/Images/StoryPictures/{0}", url.GenerateUrlId((new Random()).Next(1, 1001), storyTitle));
                string fullFolderPath = Server.MapPath(folderPath);
                if(!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                string filePath = string.Format("{0}/{1}", folderPath, httpPostedFileBase.FileName);
                string fullFilePath = string.Format("{0}/{1}", fullFolderPath, httpPostedFileBase.FileName);
                httpPostedFileBase.SaveAs(fullFilePath);
                return filePath;
            }
            else
            {
                return "/Images/StoryPictures/default/cover.jpg";
            }
        }
    }
}