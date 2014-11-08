namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;

    public class SeriesController : BaseController
    {
        public SeriesController(ITellerData data)
            : base(data)
        {
        }

        // ~/series/{id}
        public ActionResult Index(string id)
        {
            // Get series with id = {id}
            return View();
        }
    }
}