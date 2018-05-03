using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository
{
    /// <summary>
    /// Declares de demands Interface
    /// </summary>
    public interface IDemandsRepository
    {
        /// <summary>
        /// Gets the list of demands the user can read
        /// </summary>
        /// <param name="user">The user wich requested the list</param>
        /// <returns>A list with demands</returns>
        Task<List<ServiceDemand>> GetDemandsAsync(ClaimsPrincipal user);
    }
}
