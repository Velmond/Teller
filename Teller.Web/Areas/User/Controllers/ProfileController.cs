namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Web.Controllers;

    public class ProfileController : BaseController
    {
        // ~/profile/user/stories/{username}
        public ActionResult Stories(string username)
        {
            // Get user's stories page
            return View();
        }

        // ~/profile/user/series/{username}
        public ActionResult Series(string username)
        {
            // Get user's series page
            return View();
        }

        // ~/profile/user/info/{username}
        public ActionResult Info(string username)
        {
            // Get user's info page
            return View();
        }
    }
}