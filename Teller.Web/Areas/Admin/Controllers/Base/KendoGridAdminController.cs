namespace Teller.Web.Areas.Admin.Controllers.Base
{
    using System.Collections;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Teller.Data;

    public abstract class KendoGridAdminController : AdminController
    {
        public KendoGridAdminController(ITellerData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var data = this.GetData()
                .ToDataSourceResult(request);

            return this.Json(data);
        }

        ////protected virtual object Update(object model, object id)
        ////{
        ////    if(model != null && ModelState.IsValid)
        ////    {
        ////        var dbmodel = this.GetById(id);
        ////        //Mapper.Map(model, dbmodel);
        ////        this.Data.SaveChanges();
        ////    }
        ////}

        protected abstract IEnumerable GetData();

        protected abstract object GetById(object id);

        [NonAction]
        protected virtual T Create<T>(object model) where T : class
        {
            if (model != null && ModelState.IsValid)
            {
                ////var model = Mapper.Map<T>(model);
                ////var entry = this.Data.Context.Entry(model);
                ////entry.State = EntityState.Added;
                this.Data.SaveChanges();
                ////return model;
            }

            return null;
        }

        protected JsonResult GridOperation<T>(T model, [DataSourceRequest]DataSourceRequest request)
        {
            return this.Json((new[] { model }).ToDataSourceResult(request, this.ModelState));
        }

        private void ChangeEntityState(object model, EntityState state)
        {
        }
    }
}