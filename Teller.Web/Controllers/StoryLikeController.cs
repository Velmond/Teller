﻿namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Helpers;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Like;

    public class StoryLikeController : BaseController
    {
        public StoryLikeController(ITellerData data)
            : base(data)
        {
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