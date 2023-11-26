using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EZFormsPrototype.App_Start.Startup))]
namespace EZFormsPrototype.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}