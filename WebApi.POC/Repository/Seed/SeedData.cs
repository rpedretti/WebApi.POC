using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApi.POC.Domain;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Seed
{
    public static class SeedData
    {
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
