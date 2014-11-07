using System.Web.Mvc;

namespace Teller.Web.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}",
                new
                {
                    Controller = "Admin",
                    action = "Index"
                }
            );
        }
    }
}