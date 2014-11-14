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

        protected abstract IEnumerable GetData();

        protected abstract T GetById<T>(object id) where T : class;

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var data = this.GetData().ToDataSourceResult(request);

            return this.Json(data);
        }

        [NonAction]
        protected virtual T Create<T>(object model) where T : class
        {
            if (model != null && ModelState.IsValid)
            {
                //var dbModel = Mapper.Map<T>(model);
                //this.ChangeEntityStateAndSave(dbModel, EntityState.Added);
                //return dbModel;
            }

            return null;
        }

        [NonAction]
        protected virtual void Update<TModel, TViewModel>(TViewModel model, object id)
            //where TModel : AuditInfo
            //where TViewModel : AdministrationViewModel
        {
            if (model != null && ModelState.IsValid)
            {
                //var dbModel = this.GetById<TModel>(id);
                //Mapper.Map<TViewModel, TModel>(model, dbModel);
                //this.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
                //model.ModifiedOn = dbModel.ModifiedOn;
            }
        }

        [NonAction]
        protected JsonResult GridOperation<T>(T model, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [NonAction]
        private void ChangeEntityStateAndSave(object dbModel, EntityState state)
        {
            var entry = this.Data.Context.Entry(dbModel);
            entry.State = state;
            this.Data.SaveChanges();
        }
    }
}