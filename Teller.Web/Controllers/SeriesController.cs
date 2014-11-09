namespace Teller.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.ViewModels;
    using Teller.Web.ViewModels.Series;

    public class SeriesController : BaseController
    {
        public SeriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id)
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            var model = new SeriesCreateViewModel();

            model.GenresList = new SelectViewModel()
            {
                List = this.Data.Genres.All()
                    .Select(g => new SelectListItem()
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
            };

            return PartialView("_SeriesCreateFormPartial", model);
        }
    }
}