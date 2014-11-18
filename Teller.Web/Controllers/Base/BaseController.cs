namespace Teller.Web.Controllers.Base
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Teller.Data.UnitsOfWork;
    using Teller.Models;

    [HandleError]
    public abstract class BaseController : Controller
    {
        public BaseController(ITellerData data)
        {
            this.Data = data;
        }

        public ITellerData Data { get; set; }

        public AppUser UserProfile { get; set; }

        [NonAction]
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.Data.Users.All().FirstOrDefault(u => u.UserName == requestContext.HttpContext.User.Identity.Name);

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}