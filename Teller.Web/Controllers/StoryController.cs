namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Web.ViewModels;

    public class StoryController : BaseController
    {
        // ~/story/{id}
        public ActionResult Index(string id)
        {
            // get story with id = {id}
            return View();
        }

        // ~/story/new
        [Authorize]
        public ActionResult Create()
        {
            // get form for creating story
            return View();
        }

        // ~/story/new
        [Authorize]
        [HttpPost]
        public ActionResult Create(StoryViewModel story)
        {
            // add new story to database
            return View();
        }
    }
}