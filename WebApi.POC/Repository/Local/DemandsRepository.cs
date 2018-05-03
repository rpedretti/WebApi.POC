using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local
{
    /// <summary>
    /// Class responsible for dealing with demands
    /// </summary>
    public class DemandsRepository : IDemandsRepository
    {
        private readonly PocDbContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">DbContext instance</param>
        public DemandsRepository(PocDbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Gets the list of demands the user can read
        /// </summary>
        /// <param name="user">The user wich requested the list</param>
        /// <returns>A list with demands</returns>
        public async Task<List<ServiceDemand>> GetDemandsAsync(ClaimsPrincipal user)
        {
            List<ServiceDemand> demands;

            var username = user.FindFirst(ClaimTypes.NameIdentifier).Value;

            var services = _dbContext.ServiceDemands
                    .Include(s => s.Owner)
                    .Include(s => s.Status);

            if (user.IsInRole("Admin"))
            {
                demands = await services.AsNoTracking().ToListAsync();
            }
            else
            {
                demands = await services.AsNoTracking().Where(d => d.Owner.Username == username && d.IsPrivate == false).ToListAsync();
            }

            return demands;
        }
    }
}
