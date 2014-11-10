using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teller.Data;
using Teller.Web.Controllers;

namespace Teller.Web.Areas.Admin.Controllers
{
    public class CommentsController : AdminController
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