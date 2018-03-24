using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Client.Shared.Services
{
    public interface ITestAccessService
    {
        Task<bool> CallUserApiAsync();
        Task<bool> CallAdminApiAsync();
    }
}
