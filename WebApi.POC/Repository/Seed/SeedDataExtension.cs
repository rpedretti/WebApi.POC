using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApi.POC.Domain;
using WebApi.POC.Repository.Local;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Seed
{
    /// <summary>
    /// Extension methods for seeding data to the database
    /// </summary>
    public static class SeedDataExtension
    {
        /// <summary>
        /// Seeds the given database
        /// </summary>
        /// <param name="context">The database to be seeded</param>
        public static void Seed(this PocDbContext context)
        {
            context.Database.Migrate();

            if (!context.Set<Role>().Any())
            {
                context.AddRange(Role.List());
                context.AddRange(Status.List());
                context.AddRange(KeyKind.List());

                context.SaveChanges();
            }
        }
    }
}
