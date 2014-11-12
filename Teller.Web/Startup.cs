using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Teller.Web.Startup))]

namespace Teller.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
