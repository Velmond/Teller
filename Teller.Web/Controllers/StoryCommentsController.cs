namespace Teller.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Infrastructure;
    using Teller.Web.Models;
    using Teller.Web.ViewModels.Like;
    using Teller.Web.ViewModels.Story;

    public class StoryCommentsController : BaseController
    {
        public StoryCommentsController(ITellerData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Post(PostComment newComment)
        {
            if (string.IsNullOrEmpty(newComment.CommentContent))
            {
                ModelState.AddModelError("CommentContent", "Message content is required to post a message... Duh o.O");

                return this.PartialView(
                    "_CommentPartial",
                    new CommentViewModel()
                    {
                        Author = " ",
                        Content = "ERROR! Incorrect input! comment must be between 2 and 1000 characters long.",
                        DislikesCount = 0,
                        LikesCount = 0,
                        Published = DateTime.Now,
                        IsFlagged = true,
                        Id = 0
                    });
            }

            if (newComment.CommentContent.Length < 2 || newComment.CommentContent.Length > 1000)
            {
                ModelState.AddModelError("CommentContent", "Message content must be between 2 and 1000 characters long");

                return this.PartialView(
                    "_CommentPartial",
                    new CommentViewModel()
                    {
                        Author = " ",
                        Content = "ERROR! Incorrect input! comment must be between 2 and 1000 characters long.",
                        DislikesCount = 0,
                        LikesCount = 0,
                        Published = DateTime.Now,
                        IsFlagged = true,
                        Id = 0
                    });
            }

            var comment = new Comment()
            {
                AuthorId = this.User.Id,
                Content = newComment.CommentContent,
                StoryId = newComment.StoryId,
                Published = DateTime.Now
            };

            this.Data.Comments.Add(comment);
            this.Data.SaveChanges();

            var commentModel = new CommentViewModel()
            {
                Author = comment.Author.UserName,
                Content = comment.Content,
                DislikesCount = 0,
                LikesCount = 0,
                Published = comment.Published,
                IsFlagged = false,
                Id = comment.Id
            };

            return this.PartialView("_CommentPartial", commentModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Flag(int id)
        {
            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            comment.IsFlagged = true;
            this.Data.SaveChanges();

            return this.Content(string.Empty);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.RedirectToAction("Index", "Error", new { Area = string.Empty });
            }

            this.Data.Comments.Delete(comment);
            this.Data.SaveChanges();

            return this.Content(string.Empty);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Like(string id, bool like)
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

            this.Data.Likes.Add(new Like()
            {
                Value = like,
                AuthorId = this.User.Id,
                StoryId = storyId
            });

            this.Data.SaveChanges();

            var likesCount = story.Likes.Count(l => l.Value == true);
            var dislikesCount = story.Likes.Count(l => l.Value == false);
            var likesPersentage = likesCount / (likesCount + dislikesCount) * 100;

            var likesModel = new StoryLikeViewModel()
            {
                LikesCount = likesCount,
                DislikesCount = dislikesCount,
                LikesPersentage = likesPersentage
            };

            return this.PartialView("_StoryLikes", likesModel);
        }
    }
}