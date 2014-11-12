namespace Teller.Web.Areas.User
{
    using System.Web.Mvc;

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
                defaults: new
                {
                    controller = "Users",
                    action = "Index"
                },
                namespaces: new string[] { "Teller.Web.Areas.User.Controllers" });

            context.MapRoute(
                name: "User_default",
                url: "User/{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Profile",
                    action = "Edit"
                },
                namespaces: new string[] { "Teller.Web.Areas.User.Controllers" });
        }
    }
}