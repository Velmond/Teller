namespace Teller.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;

    public abstract class BaseController : Controller
    {
        public BaseController(ITellerData data)
        {
            this.Data = data;
            this.User = System.Web.HttpContext.Current.Session["user"] as AppUser;

            if (this.User == null)
            {
                this.User = this.Data.Users.All()
                    .FirstOrDefault(u => u.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                System.Web.HttpContext.Current.Session.Add("user", this.User);
            }
        }

        public ITellerData Data { get; set; }

        public AppUser User { get; set; }

        [NonAction]
        public void SytemSettings()
        {
        }
    }
}