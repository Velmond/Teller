namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Web.ViewModels;

    public class FeedController : BaseController
    {
        [Authorize]
        public ActionResult Index(string username)
        {
            var feedStoriesCollections = this.User.SubscribedTo.Select(s => s.Stories);
            var feedStories = new List<UserFeedStory>();

            foreach(var collection in feedStoriesCollections)
            {
                feedStories.AddRange(collection.Select(UserFeedStory.FromStory));
            }

            feedStories = feedStories.OrderByDescending(s => s.DatePublished).ToList();

            return View(feedStories);
        }
    }
}
