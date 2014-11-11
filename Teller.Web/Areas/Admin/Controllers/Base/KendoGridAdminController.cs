namespace Teller.Web.Areas.Admin.Controllers.Base
{
    using Teller.Data;

    public abstract class KendoGridAdminController : AdminController
    {
        public KendoGridAdminController(ITellerData data)
            : base(data)
        {
        }

    }
}