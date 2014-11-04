namespace Teller.Web.Controllers
{
    using System.Web.Mvc;
    using Teller.Models;

    public abstract class BaseController : Controller
    {
        public AppUser User { get; set; }

        [NonAction]
        public void SytemSettings()
        {

        }
    }
}