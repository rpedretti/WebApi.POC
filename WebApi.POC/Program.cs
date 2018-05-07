using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using WebApi.POC.Domain;
using WebApi.POC.Repository.Local;
using WebApi.Shared.Domain;

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
                var sessionFactory = services.GetRequiredService<NHSessionFactory>();
                using (var session = sessionFactory.SessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        if (!session.Query<KeyKind>().Any())
                        {
                            foreach (var kind in KeyKind.List())
                            {
                                session.Save(kind);
                            }
                            foreach (var role in Role.List())
                            {
                                session.Save(role);
                            }

                            foreach (var status in Status.List())
                            {
                                session.Save(status);
                            }

                            session.Flush();
                            tx.CommitAsync();
                        }
                    };

                    if (!hostEnv.IsEnvironment("SwaggerMock"))
                    {
                        try
                        {
                            MockData.Initialize(session).Wait();
                        }
                        catch (Exception ex)
                        {
                            var logger = services.GetRequiredService<ILogger<Program>>();
                            logger.LogError(ex, "An error occurred while seeding the database.");
                        }
                    }
                    if (!hostEnv.IsProduction())
                    {
                        sessionFactory.ShowSqlLog(true);
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
