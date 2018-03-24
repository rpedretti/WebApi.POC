using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Shared.Constants;
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

        public async Task<bool> CallUserApiAsync()
        {
            try
            {
                var message = new SecureMessageModel
                {
                    FromId = 1,
                    Message = "I'm a simple user"
                };
                var result = await _securityService.SendMessageOnSecureChannelAsync(message, "api/test/sayencryptedhello");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CallAdminApiAsync()
        {
            try
            {
                var message = new SecureMessageModel
                {
                    FromId = 1,
                    Message = "I'm a admin user"
                };

                var result = await _securityService.SendMessageOnSecureChannelAsync(message, "api/test/sayencryptedhelloadmin");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
