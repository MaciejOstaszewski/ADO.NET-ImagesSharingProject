using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImagesShareingProject.Startup))]
namespace ImagesShareingProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
