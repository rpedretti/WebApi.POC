using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.Client.Shared.Services
{
    public sealed class TestAccessService : ITestAccessService
    {
        private ISecureChannelService _securityService;

        public TestAccessService(ISecureChannelService securityService)
        {
            _securityService = securityService;
        }

        public async Task<List<ServiceDemand>> GetDemands()
        {
            List<ServiceDemand> demands = new List<ServiceDemand>();
            try
            {
                demands = await _securityService.GetOnSecureChannelAsync<List<ServiceDemand>>("api/test/getDemands");
            }
            catch (UnauthorizedAccessException)
            {
                System.Diagnostics.Debug.WriteLine("unauthorized");
            }
            return demands;
        }
    }
}
