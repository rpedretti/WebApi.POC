using NHibernate;
using NHibernate.Linq;
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
    public class DemandsRepository : BaseNHContext, IDemandsRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        public DemandsRepository(ISession session) : base(session)
        {
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

            var services = Read<ServiceDemand>()
                .Fetch(s => s.Owner)
                .ThenFetch(u => u.Role)
                .Fetch(s => s.Status);

            if (user.IsInRole("Admin"))
            {
                demands = await services.ToListAsync();
            }
            else
            {
                demands = await services.Where(d => d.Owner.Username == username && d.IsPrivate == false).ToListAsync();
            }

            return demands;
        }
    }
}
