using System.Web.Mvc;

namespace Teller.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                namespaces: new string[] { "Teller.Web.Areas.Admin.Controllers" },
                defaults: new {
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}