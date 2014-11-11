namespace Teller.Web.Areas.Admin.Controllers.Base
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using Teller.Data;
    using System.Data.Entity;

    public abstract class KendoGridAdminController : AdminController
    {
        public KendoGridAdminController(ITellerData data)
            : base(data)
        {
        }

        protected abstract IEnumerable GetData();

        protected abstract object GetById(object id);

        private void ChangeEntityState(object model, EntityState state)
        {

        }

        //protected virtual object Update(object model, object id)
        //{
        //    if(model != null && ModelState.IsValid)
        //    {
        //        var dbmodel = this.GetById(id);
        //        //Mapper.Map(model, dbmodel);
        //        this.Data.SaveChanges();
        //    }
        //}

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var data = this.GetData()
                .ToDataSourceResult(request);

            return this.Json(data);
        }

        [NonAction]
        protected virtual T Create<T>(object model) where T: class
        {
            if(model != null && ModelState.IsValid)
            {
                //var model = Mapper.Map<T>(model);
                //var entry = this.Data.Context.Entry(model);
                //entry.State = EntityState.Added;
                this.Data.SaveChanges();
                //return model;
            }

            return null;
        }

        protected JsonResult GridOperation<T>(T model, [DataSourceRequest]DataSourceRequest request)
        {
            return Json((new[] { model }).ToDataSourceResult(request, ModelState));
        }
    }
}