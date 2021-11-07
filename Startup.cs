using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YTUsageViewer.Startup))]
namespace YTUsageViewer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
