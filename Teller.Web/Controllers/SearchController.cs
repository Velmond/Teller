namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class SearchController : BaseController
    {
        // ~/search/{pattern}
        public ActionResult Index(string pattern)
        {
            // Get stories by title, series by name and users by username
            return View();
        }
    }
}