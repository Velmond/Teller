namespace Teller.Web.Controllers
{
    using System.Web.Mvc;
    using Teller.Web.ViewModels;

    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult NotFound()
        {
            return this.View();
        }

        public ActionResult ServerError()
        {
            return this.View();
        }
    }
}