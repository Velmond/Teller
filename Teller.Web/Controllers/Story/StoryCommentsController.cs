namespace Teller.Web.Controllers.Story
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data.UnitsOfWork;
    using Teller.Models;
    using Teller.Web.Controllers.Base;
    using Teller.Web.Infrastructure.UrlGeneratotrs;
    using Teller.Web.Models;
    using Teller.Web.ViewModels.Like;
    using Teller.Web.ViewModels.Story;

    public class StoryCommentsController : BaseController
    {
        private const string ContentIsRequiredMsg = "Message content is required to post a message... Duh o.O";
        private const string ContentTooShortOrTooLongMsg = "Comment must be between 2 and 1000 characters long.";
        private const string CommentPartialName = "_CommentPartial";
        private const string StoryLikesPartialName = "_StoryLikes";

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
                ModelState.AddModelError("CommentContent", ContentIsRequiredMsg);

                return this.PartialView(
                    CommentPartialName,
                    new CommentViewModel()
                    {
                        Author = " ",
                        Content = ContentTooShortOrTooLongMsg,
                        DislikesCount = 0,
                        LikesCount = 0,
                        Published = DateTime.Now,
                        IsFlagged = true,
                        Id = 0
                    });
            }

            if (newComment.CommentContent.Length < 2 || newComment.CommentContent.Length > 1000)
            {
                ModelState.AddModelError("CommentContent", ContentTooShortOrTooLongMsg);

                return this.PartialView(
                    CommentPartialName,
                    new CommentViewModel()
                    {
                        Author = " ",
                        Content = ContentTooShortOrTooLongMsg,
                        DislikesCount = 0,
                        LikesCount = 0,
                        Published = DateTime.Now,
                        IsFlagged = true,
                        Id = 0
                    });
            }

            var comment = new Comment()
            {
                AuthorId = this.UserProfile.Id,
                Content = newComment.CommentContent,
                StoryId = newComment.StoryId,
                Published = DateTime.Now
            };

            this.Data.Comments.Add(comment);
            this.Data.SaveChanges();

            var commentModel = new CommentViewModel()
            {
                Author = this.UserProfile.UserName,
                Content = comment.Content,
                DislikesCount = 0,
                LikesCount = 0,
                Published = comment.Published,
                IsFlagged = false,
                Id = comment.Id
            };

            return this.PartialView(CommentPartialName, commentModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Flag(int id)
        {
            var comment = this.Data.Comments.GetById(id);

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
            var comment = this.Data.Comments.GetById(id);

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

            var story = this.Data.Stories.GetById(storyId);

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
                AuthorId = this.UserProfile.Id,
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

            return this.PartialView(StoryLikesPartialName, likesModel);
        }
    }
}