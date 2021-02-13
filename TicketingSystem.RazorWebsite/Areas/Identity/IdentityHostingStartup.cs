using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TicketingSystem.RazorWebsite.Areas.Identity.IdentityHostingStartup))]
namespace TicketingSystem.RazorWebsite.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}