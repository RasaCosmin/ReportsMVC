using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCReports.Startup))]
namespace MVCReports
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
