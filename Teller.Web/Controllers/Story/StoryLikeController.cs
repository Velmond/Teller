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
    using Teller.Web.ViewModels.Like;

    public class StoryLikeController : BaseController
    {
        private const string StoryLikesPartialName = "_StoryLikes";

        public StoryLikeController(ITellerData data)
            : base(data)
        {
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
                throw new HttpException(400, "Invalid story.");
            }

            var story = this.Data.Stories.GetById(storyId);

            if (story == null)
            {
                throw new HttpException(400, "Invalid story.");
            }

            var url = new UrlGenerator();
            var encodedStoryId = url.GenerateUrlId(story.Id, story.Title);

            if (encodedStoryId != id)
            {
                throw new HttpException(404, "Story was not found in the database");
            }

            story.Likes.Add(new Like
            {
                Value = like,
                AuthorId = this.UserProfile.Id
            });

            this.Data.SaveChanges();

            var likesCount = story.Likes.Count(l => l.Value == true);
            var dislikesCount = story.Likes.Count(l => l.Value == false);

            var likesModel = new StoryLikeViewModel()
            {
                LikesCount = likesCount,
                DislikesCount = dislikesCount,
                LikesPersentage = (int)((double)likesCount / (likesCount + dislikesCount) * 100)
            };

            return this.PartialView(StoryLikesPartialName, likesModel);
        }
    }
}
