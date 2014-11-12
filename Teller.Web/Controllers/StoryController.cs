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
    using Teller.Web.Infrastructure;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Series;
    using Teller.Web.ViewModels.Story;

    public class StoryController : BaseController
    {
        private const string DefaultStoryImage = "/Images/StoryPictures/default/cover.jpg";
        private readonly ISanitizer sanitizer;

        public StoryController(ITellerData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var foundStory = this.Data.Stories.Find(storyId);

            if (foundStory == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(foundStory.Id, foundStory.Title);

            if (encodedStoryId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
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

            if (foundStory.Series != null)
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

            return this.View(story);
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View(this.GetStoryFormModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FormViewModel story)
        {
            if (!string.IsNullOrEmpty(story.SeriesName) &&
                !string.IsNullOrWhiteSpace(story.SeriesName))
            {
                if (story.SeriesName.Length < 2)
                {
                    ModelState.AddModelError("SeriesName", "Series name must be at least 2 characters long.");
                }
                else if (!story.SeriesGenreId.HasValue)
                {
                    ModelState.AddModelError("SeriesGenreId", "New series must have genre selected.");
                }
            }

            if (ModelState.IsValid)
            {
                Story newStory = new Story()
                {
                    Title = story.Title,
                    AuthorId = this.User.Id,
                    Content = this.sanitizer.Sanitize(story.Content),
                    DatePublished = DateTime.Now,
                    GenreId = story.GenreId
                };

                newStory.SeriesId = this.GetSeriesId(story);
                newStory.PicturePath = this.GetPicturePath(story.Picture, story.Title);

                this.Data.Stories.Add(newStory);
                this.Data.SaveChanges();

                var url = new UrlGenerator();
                return this.RedirectToAction("Index", new { id = url.GenerateUrlId(newStory.Id, newStory.Title) });
            }

            var model = this.GetStoryFormModel();
            model.Title = string.IsNullOrEmpty(story.Title) ? string.Empty : story.Title;
            model.Content = string.IsNullOrEmpty(story.Content) ? string.Empty : story.Title;
            model.Picture = story.Picture;
            model.GenreId = story.GenreId;
            model.SeriesId = story.GenreId;
            model.SeriesGenreId = story.SeriesGenreId;
            model.SeriesName = string.IsNullOrEmpty(story.SeriesName) ? string.Empty : story.Title;

            return this.View(model);
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var foundStory = this.Data.Stories.Find(storyId);

            if (foundStory == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(foundStory.Id, foundStory.Title);

            if (encodedStoryId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var story = this.GetStoryFormModel();

            story.Title = foundStory.Title;
            story.Content = foundStory.Content;
            story.GenreId = foundStory.GenreId;
            story.SeriesId = foundStory.SeriesId;

            return this.View(story);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(string id, FormViewModel story)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var foundStory = this.Data.Stories.Find(storyId);

            if (foundStory == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(foundStory.Id, foundStory.Title);

            if (encodedStoryId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            if (!string.IsNullOrEmpty(story.SeriesName) &&
                !string.IsNullOrWhiteSpace(story.SeriesName))
            {
                if (story.SeriesName.Length < 2)
                {
                    ModelState.AddModelError("SeriesName", "Series name must be at least 2 characters long.");
                }
                else if (!story.SeriesGenreId.HasValue)
                {
                    ModelState.AddModelError("SeriesGenreId", "New series must have genre selected.");
                }
            }

            if (ModelState.IsValid)
            {
                foundStory.Title = story.Title;
                foundStory.Content = this.sanitizer.Sanitize(story.Content);
                foundStory.GenreId = story.GenreId;
                foundStory.SeriesId = this.GetSeriesId(story);

                var newPicturePath = this.GetPicturePath(story.Picture, story.Title);

                if (!string.IsNullOrEmpty(newPicturePath) &&
                    !string.IsNullOrWhiteSpace(newPicturePath) &&
                    newPicturePath != DefaultStoryImage)
                {
                    foundStory.PicturePath = newPicturePath;
                }

                this.Data.Stories.Update(foundStory);
                this.Data.SaveChanges();

                return this.RedirectToAction("Index", new { id = url.GenerateUrlId(foundStory.Id, foundStory.Title) });
            }

            var model = this.GetStoryFormModel();
            model.Title = string.IsNullOrEmpty(story.Title) ? string.Empty : story.Title;
            model.Content = string.IsNullOrEmpty(story.Content) ? string.Empty : story.Title;
            model.Picture = story.Picture;
            model.GenreId = story.GenreId;
            model.SeriesId = story.GenreId;
            model.SeriesGenreId = story.SeriesGenreId;
            model.SeriesName = string.IsNullOrEmpty(story.SeriesName) ? string.Empty : story.Title;

            return this.View(model);
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var story = this.Data.Stories.Find(storyId);

            if (story == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if (encodedStoryId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            this.Data.Stories.Delete(story);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Home", new { Area = string.Empty });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Flag(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var story = this.Data.Stories.Find(storyId);

            if (story == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if (encodedStoryId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
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
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var story = this.Data.Stories.Find(storyId);

            if (story == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if (encodedStoryId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            if (this.User.Favourites.Any(s => s == story))
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
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id) || id.IndexOf('-') < 0)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            var story = this.Data.Stories.Find(storyId);

            if (story == null)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if (encodedStoryId != id)
            {
                return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            if (this.User.ReadLater.Any(s => s == story))
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

        [NonAction]
        private FormViewModel GetStoryFormModel()
        {
            var model = new FormViewModel();

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
            if (story.SeriesId != null)
            {
                return story.SeriesId;
            }
            else if (story.SeriesName != null && story.SeriesGenreId.HasValue)
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
            if (httpPostedFileBase != null && httpPostedFileBase.ContentType.StartsWith("image/"))
            {
                var url = new UrlGenerator();

                string folderPath = string.Format("/Images/StoryPictures/{0}", url.GenerateUrlId((new Random()).Next(1, 1001), storyTitle));
                string fullFolderPath = Server.MapPath(folderPath);
                if (!Directory.Exists(fullFolderPath))
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
                return DefaultStoryImage;
            }
        }
    }
}