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

            //routes.MapRoute(
            //    name: "Feed",
            //    url: "{controller}/{username}",
            //    defaults: new
            //    {
            //        controller = "Feed",
            //        action = "Index"
            //    }
            //);

            //routes.MapRoute(
            //    name: "CreateStory",
            //    url: "Story/Create",
            //    defaults: new
            //    {
            //        controller = "Story",
            //        action = "Create"
            //    }
            //);

            //routes.MapRoute(
            //    name: "Search",
            //    url: "{controller}/{action}"
            //);

            //routes.MapRoute(
            //    name: "WithUsername",
            //    url: "{controller}/{action}/{username}",
            //    namespaces: new string[] { "Teller.Web.Controllers" },
            //    defaults: new
            //    {
            //        controller = "Feed",
            //        action = "Index"
            //    }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                namespaces: new string[] { "Teller.Web.Controllers" },
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            //routes.MapRoute(
            //    name: "Profile",
            //    url: "{area}/{controller}/{action}/{username}",
            //    defaults: new
            //    {
            //        area = "User",
            //        controller = "Profile",
            //        action = "Stories",
            //        username = UrlParameter.Optional
            //    }
            //);
        }
    }
}
