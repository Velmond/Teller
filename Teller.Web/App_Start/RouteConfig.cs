using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Teller.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "StaicPages",
            //    url: "{action}",
            //    defaults: new { controller = "Home" }
            //);

            routes.MapRoute(
                name: "Feed",
                url: "{controller}/{action}/{username}",
                defaults: new
                {
                    controller = "Feed",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Profile",
                url: "{area}/{controller}/{action}/{username}",
                defaults: new
                {
                    area = "User",
                    controller = "Profile",
                    action = "Stories",
                    username = UrlParameter.Optional
                }
            );
        }
    }
}
