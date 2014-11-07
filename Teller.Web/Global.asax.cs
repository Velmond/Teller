using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Teller.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpException lastErrorWrapper = Server.GetLastError() as HttpException;

            if(lastErrorWrapper.GetHttpCode() == 404)
            {
                Server.Transfer("~/Error/NotFound");
            }
            else
            {
                //Server.Transfer("~/Error/Oops");
            }

            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if(httpException != null)
            {
                string action;

                switch(httpException.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;
                    case 500:
                        action = "ServerError";
                        break;
                    default:
                        action = "Oops";
                        break;
                }

                Server.ClearError();

                //Response.Redirect(string.Format("~/Error/{0}/?message={1}&stack={2}", action, exception.Message, exception.StackTrace));
                Response.Redirect(string.Format("~/Error/{0}", action));
            }
        }
    }
}
