namespace Teller.Web.Areas.Admin.Controllers.Base
{
    using System.Collections;
    using System.Data.Entity;
    using System.Web.Mvc;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    
    using Teller.Common;
    using Teller.Data.UnitsOfWork;
    using Teller.Web.Controllers.Base;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public abstract class AdminController : BaseController
    {
        public AdminController(ITellerData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var data = this.GetData().ToDataSourceResult(request);

            return this.Json(data);
        }

        protected abstract IEnumerable GetData();

        protected abstract T GetById<T>(object id) where T : class;

        [NonAction]
        protected JsonResult GridOperation<T>(T model, [DataSourceRequest]DataSourceRequest request)
        {
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [NonAction]
        protected void ChangeEntityStateAndSave(object dbModel, EntityState state)
        {
            var entry = this.Data.Context.Entry(dbModel);
            entry.State = state;
            this.Data.SaveChanges();
        }
    }
}