using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GuestBookProject.Startup))]
namespace GuestBookProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
