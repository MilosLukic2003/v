using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NektarPodgorine.Web.Startup))]

namespace NektarPodgorine.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
