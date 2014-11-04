using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Teller.Web.Controllers
{
    public class FeedController : Controller
    {
        // ~/feed/{id}
        public ActionResult Index(string id)
        {
            // Get user's subscription feed
            return View();
        }
    }
}
