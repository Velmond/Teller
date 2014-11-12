namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.ViewModels.Story;

    ////using Model = Teller.Models.AppUser;
    ////using ViewModels = Teller.Web.Areas.Admin.ViewModels.User;

    public class UsersController : AdminController
    {
        public UsersController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        ////protected ActionResult Create([DataSourceRequest]DataSourceRequest request, AppUser model)
        ////{
        ////    var dbmodel = base.Create<AppUser>(model);
        ////    if(dbmodel != null)
        ////    {
        ////        model.Id = dbmodel.Id;
        ////    }

        ////    return base.GridOperation(model, request);
        ////}

        ////protected override IEnumerable GetData()
        ////{
        ////    return this.Data.Users.All();
        ////}
    }
}
