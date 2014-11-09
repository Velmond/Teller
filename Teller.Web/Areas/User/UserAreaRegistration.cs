using System.Web.Mvc;

namespace Teller.Web.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "User_NoAction",
                url: "User/{controller}/{id}",
                namespaces: new string[] { "Teller.Web.Areas.User.Controllers" },
                defaults: new
                {
                    controller = "Users",
                    action = "Index"
                }
            );

            context.MapRoute(
                name: "User_default",
                url: "User/{controller}/{action}/{id}",
                namespaces: new string[] { "Teller.Web.Areas.User.Controllers" },
                defaults: new
                {
                    controller = "Profile",
                    action = "Edit"
                }
            );
        }
    }
}