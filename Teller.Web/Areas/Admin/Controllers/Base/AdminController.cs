namespace Teller.Web.Areas.Admin.Controllers.Base
{
    using System.Web.Mvc;

    using Teller.Common;
    using Teller.Data;
    using Teller.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public abstract class AdminController : BaseController
    {
        public AdminController(ITellerData data)
            : base(data)
        {
        }
    }
}