[assembly: HostingStartup(
    typeof(AspNetCoreOpeniddictPlus.Web.Areas.Identity.IdentityHostingStartup)
)]

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { });
    }
}
