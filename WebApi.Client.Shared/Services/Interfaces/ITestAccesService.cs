using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.Client.Shared.Services
{
    public interface ITestAccessService
    {
        Task<List<ServiceDemand>> GetDemands();
    }
}
