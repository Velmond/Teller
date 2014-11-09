﻿namespace Teller.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Helpers;
    using Teller.Web.Models;
    using Teller.Web.ViewModels.Story;
    using Teller.Web.ViewModels.Like;

    public class StoryCommentsController : BaseController
    {
        public StoryCommentsController(ITellerData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(PostComment newComment)
        {
            if(!ModelState.IsValid)
            {
                return Redirect("~/Error");
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

            return PartialView("_CommentPartial", commentModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Flag(int id)
        {
            var comment = this.Data.Comments.Find(id);

            if(comment == null)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            comment.IsFlagged = true;
            this.Data.SaveChanges();

            return Content("");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var comment = this.Data.Comments.Find(id);

            if(comment == null)
            {
                return RedirectToAction("Index", "Error", new { Area = "" });
            }

            this.Data.Comments.Delete(comment);
            this.Data.SaveChanges();

            return Content("");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Like(string id, bool like)
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

            this.Data.Likes.Add(new Like()
            {
                Value = like,
                AuthorId = this.User.Id,
                StoryId = storyId
            });

            this.Data.SaveChanges();

            var likesCount = story.Likes.Count(l => l.Value == true);
            var dislikesCount = story.Likes.Count(l => l.Value == false);
            var likesPersentage = (likesCount / (likesCount + dislikesCount) * 100);

            var likesModel = new StoryLikeViewModel()
            {
                LikesCount = likesCount,
                DislikesCount = dislikesCount,
                LikesPersentage = likesPersentage
            };

            return PartialView("_StoryLikes", likesModel);
        }
    }
}