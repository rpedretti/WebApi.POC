using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository
{
    /// <summary>
    /// Mocked class responsible for dealing with demands
    /// </summary>
    public class DemandsRepositoryMock : IDemandsRepository
    {
        /// <summary>
        /// Gets the list of demands the user can read
        /// </summary>
        /// <param name="user">The user wich requested the list</param>
        /// <returns>A list with demands</returns>
        public async Task<List<ServiceDemand>> GetDemandsAsync(ClaimsPrincipal user)
        {
            List<ServiceDemand> demands = new List<ServiceDemand>();

            return await Task.FromResult(demands);
        }
    }
}
