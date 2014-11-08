using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teller.Data;
using Teller.Web.Controllers;

namespace Teller.Web.Areas.User.Controllers
{
    public class StoriesController : BaseController
    {
        public StoriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string username)
        {
            ViewBag.Username = username;

            return View();
        }
    }
}