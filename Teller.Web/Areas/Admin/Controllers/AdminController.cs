namespace Teller.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Web.Controllers;

    [Authorize(Roles = "Admin")]
    public abstract class AdminController : BaseController
    {
        public AdminController(ITellerData data)
            : base(data)
        {
        }
    }
}