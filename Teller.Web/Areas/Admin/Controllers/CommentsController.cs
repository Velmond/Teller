namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.Areas.Admin.Controllers.Base;

    public class CommentsController : AdminController
    {
        public CommentsController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        ////protected override System.Collections.IEnumerable GetData()
        ////{
        ////    return this.Data.Comments.All();
        ////}
    }
}