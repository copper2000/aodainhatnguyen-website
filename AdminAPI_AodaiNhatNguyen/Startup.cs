using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdminAPI_AodaiNhatNguyen.Startup))]
namespace AdminAPI_AodaiNhatNguyen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
