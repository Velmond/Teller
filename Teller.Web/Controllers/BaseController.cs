namespace Teller.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;

    public abstract class BaseController : Controller
    {
        //public BaseController()
        //    : this(new TellerData(new TellerDbContext()))
        //{
        //}

        public BaseController(ITellerData data)
        {
            this.Data = data;
            this.User = this.Data.Users.All()
                .FirstOrDefault(u => u.UserName == System.Web.HttpContext.Current.User.Identity.Name);
        }

        public ITellerData Data { get; set; }

        public AppUser User { get; set; }

        [NonAction]
        public void SytemSettings()
        {

        }
    }
}