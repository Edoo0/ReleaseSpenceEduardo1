using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReleaseSpence.Startup))]
namespace ReleaseSpence
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
