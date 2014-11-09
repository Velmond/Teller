using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teller.Data;
using Teller.Web.Controllers;

namespace Teller.Web.Areas.Admin.Controllers
{
    public class AdminUsersController : AdminController
    {
        public AdminUsersController(ITellerData data)
            : base(data)
        {
        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            return View();
        }
    }
}