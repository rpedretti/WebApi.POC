using System.Threading.Tasks;

namespace WebApi.POC.Services
{
    public interface ISecurityService
    {
        Task<string> GetPublicRSAKeyAsync(string id);
        Task<string> GetPrivateRSAKeyAsync(string id);
        Task<string> GetClientPublicRSAKeysAsync(string id);
        Task SaveClientRSAKeyAsync(string id, string key);
    }
}