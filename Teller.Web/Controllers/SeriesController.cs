namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class SeriesController : BaseController
    {
        // ~/series
        public ActionResult Index()
        {
            // Get top 5-10 most popular series for each genre
            return View();
        }

        // ~/series/{id}
        public ActionResult Index(int id)
        {
            // Get series with id = {id}
            return View();
        }
    }
}