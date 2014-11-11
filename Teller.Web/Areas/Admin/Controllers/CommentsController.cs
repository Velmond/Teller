﻿namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.Areas.Admin.Controllers.Base;

    public class CommentsController : KendoGridAdminController
    {
        public CommentsController(ITellerData data)
            : base(data)
        {
        }

        // GET: Admin/Comments
        public ActionResult Index()
        {
            return View();
        }
    }
}