namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Helpers;
    using Teller.Web.Models;
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
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Story is already in your favorites list");
            }

            this.User.Favourites.Add(story);
            this.Data.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Story added to favorites list");
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
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Story is already in your 'Read later' list");
            }

            this.User.ReadLater.Add(story);
            this.Data.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Story added to 'Read later' list");
        }

        [NonAction]
        private int? GetSeriesId(StoryFormViewModel story)
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