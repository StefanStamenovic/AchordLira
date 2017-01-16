using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AchordLira.Startup))]
namespace AchordLira
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
