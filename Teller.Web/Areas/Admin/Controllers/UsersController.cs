namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    
    using Teller.Data;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.ViewModels.Story;

    public class UsersController : KendoGridAdminController
    {
        public UsersController(ITellerData data)
            : base(data)
        {
        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var data = this.Data.Stories.All()
                .Select(s => new StoryViewModel()
                {
                    Id = s.Id,
                    Author = s.Author.UserName,
                    Content = s.Content,
                    Title = s.Title,
                    //DatePublished = s.DatePublished,
                    PicturePath = s.PicturePath,
                    SeriesName = s.Series.Title
                })
                //.AsQueryable()
                .ToDataSourceResult(request, ModelState);

            return Json(data);
        }
    }
}
