namespace Teller.Web.Controllers
{
    using System.Web.Mvc;
    using Teller.Web.ViewModels;

    public class ErrorController : Controller
    {
        public ActionResult Index(/*string message, string stack*/)
        {
            //var error = new ErrorViewModel()
            //{
            //    Message = message,
            //    StackTrace = stack
            //};

            //return View(error);
            return View();
        }

        public ActionResult NotFound(/*string message, string stack*/)
        {
            //var error = new ErrorViewModel()
            //{
            //    Message = message,
            //    StackTrace = stack
            //};

            //return View(error);
            return View();
        }

        public ActionResult ServerError(/*string message, string stack*/)
        {
            //var error = new ErrorViewModel()
            //{
            //    Message = message,
            //    StackTrace = stack
            //};

            //return View(error);
            return View();
        }
    }
}