using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebApi.POC.Repository;
using WebApi.POC.Repository.Seed;

namespace WebApi.POC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var hostEnv = services.GetRequiredService<IHostingEnvironment>();
                var context = services.GetRequiredService<PocDbContext>();
                context.Seed();

                if (hostEnv.IsDevelopment())
                {
                    try
                    {
                        MockData.Initialize(context);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:1234")
                .Build();
    }
}
