using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebApi.POC.Repository.Local;
using WebApi.POC.Repository.Seed;

namespace WebApi.POC
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Program
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Main(string[] args)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var hostEnv = services.GetRequiredService<IHostingEnvironment>();

                if (!hostEnv.IsEnvironment("SwaggerMock"))
                {
                    var context = services.GetRequiredService<PocDbContext>();
                    context.Seed();
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

        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
