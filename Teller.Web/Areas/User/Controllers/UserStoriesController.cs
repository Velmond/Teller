﻿namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Web.Controllers;

    public class UserStoriesController : BaseController
    {
        public UserStoriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id)
        {
            ViewBag.Username = id;

            return View();
        }
    }
}