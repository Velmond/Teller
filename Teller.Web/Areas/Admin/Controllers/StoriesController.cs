namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Teller.Data;
    using Teller.Web.Areas.Admin.Controllers.Base;

    public class StoriesController : AdminController
    {
        public StoriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            throw new NotImplementedException();
        }

        protected override T GetById<T>(object id)
        {
            throw new NotImplementedException();
        }
    }
}