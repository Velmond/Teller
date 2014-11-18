namespace Teller.Web.App_Start
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var namespaces = new[]
                {
                    "Teller.Web.Controllers.Account",
                    "Teller.Web.Controllers.Home",
                    "Teller.Web.Controllers.Search",
                    "Teller.Web.Controllers.Series",
                    "Teller.Web.Controllers.Story"
                };

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: namespaces);
        }
    }
}
