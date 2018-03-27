using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Shared.Constants;
using WebApi.Shared.Domain;
using WebApi.Shared.Models;

namespace WebApi.Client.Shared.Services
{
    public sealed class TestAccessService : ITestAccessService
    {
        private ISecurityService _securityService;

        public TestAccessService(ISecurityService securityService)
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
