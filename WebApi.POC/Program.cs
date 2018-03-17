using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApi.POC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.ConfigureLogging((hostingContext, l) => {
                //    l.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    l.AddConsole();
                //    l.AddDebug();
                //})
                .Build();
    }
}
