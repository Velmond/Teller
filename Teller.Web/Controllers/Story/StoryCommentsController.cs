namespace Teller.Web.Controllers.Story
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    
    using Teller.Data.UnitsOfWork;
    using Teller.Models;
    using Teller.Web.Controllers.Base;
    using Teller.Web.Infrastructure.UrlGenerators;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Like;
    using Teller.Web.ViewModels.Story;

    public class StoryCommentsController : BaseController
    {
        ////private const string ContentIsRequiredMsg = "Message content is required to post a message... Duh o.O";
        ////private const string ContentTooShortOrTooLongMsg = "Comment must be between 2 and 1000 characters long.";
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
        public ActionResult Post(PostCommentViewModel newComment)
        {
            if (string.IsNullOrEmpty(newComment.CommentContent))
            {
                throw new HttpException(400, "Comment content is required.");
            }

            if (newComment.CommentContent.Length < 2 || newComment.CommentContent.Length > 1000)
            {
                throw new HttpException(400, "Comment content must be between 2 and 1000 characters long.");
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
                throw new HttpException(400, "Comment could not be found in the database.");
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
                throw new HttpException(400, "Comment could not be found in the database.");
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
                throw new HttpException(400, "Invalid story identifier.");
            }

            int storyId;
            if (!int.TryParse(id.Substring(id.LastIndexOf('-') + 1), out storyId))
            {
                throw new HttpException(400, "Invalid story identifier.");
            }

            var story = this.Data.Stories.GetById(storyId);

            if (story == null)
            {
                throw new HttpException(404, "Invalid story identifier.");
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if (encodedStoryId != id)
            {
                throw new HttpException(404, "Story could not be found.");
            }

            story.Likes.Add(new Like
            {
                Value = like,
                AuthorId = this.UserProfile.Id
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