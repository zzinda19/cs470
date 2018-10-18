using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cs470project.Startup))]
namespace cs470project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
