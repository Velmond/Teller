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
                "User_NoAction",
                "User/{controller}/{username}",
                new
                {
                    controller = "Users",
                    action = "Index"
                }
            );

            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{username}",
                new
                {
                    controller = "Profile",
                    action = "Edit"
                }
            );
        }
    }
}