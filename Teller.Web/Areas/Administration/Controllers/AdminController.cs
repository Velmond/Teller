namespace Teller.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Web.Controllers;

    [Authorize(Roles = "Admin")]
    public abstract class AdminController : BaseController
    {
        public AdminController(ITellerData data)
            : base(data)
        {
        }

        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}