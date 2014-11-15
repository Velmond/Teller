[assembly: Microsoft.Owin.OwinStartupAttribute(typeof(Teller.Web.App_Start.Startup))]

namespace Teller.Web.App_Start
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
