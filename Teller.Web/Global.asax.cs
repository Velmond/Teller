namespace Teller.Web
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Teller.Web.Infrastructure.Mapping;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEnginesConfig.RegisterViewEngines();
            AutoMapperConfig.Execute();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        ////protected void Application_Error(object sender, EventArgs e)
        ////{
        ////    HttpException lastError = Server.GetLastError() as HttpException;

        ////    Response.Clear();
        ////    Server.ClearError();

        ////    if(lastError != null)
        ////    {
        ////        if(lastError.GetHttpCode() == 404)
        ////        {
        ////            //Response.Redirect("~/Error/NotFound");
        ////            Server.Transfer("~/Error/NotFound");
        ////        }
        ////        else if(lastError.GetHttpCode() == 500)
        ////        {
        ////            //Response.Redirect("~/Error/ServerError");
        ////            Server.Transfer("~/Error/ServerError");
        ////        }
        ////        else
        ////        {
        ////            //Response.Redirect("~/Error/Oops");
        ////            Server.Transfer("~/Error/Oops");
        ////        }

        ////        //Response.Redirect(string.Format("~/Error/{0}/?message={1}&stack={2}", action, exception.Message, exception.StackTrace));
        ////    }
        ////}
    }
}
