using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pazzino.Startup))]
namespace Pazzino
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
