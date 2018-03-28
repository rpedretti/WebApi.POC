using System.Threading.Tasks;

namespace WebApi.POC.Services
{
    public interface ISecurityService
    {
        Task<string> GetPublicRSAKeyAsync(int id);
        Task<string> GetPrivateRSAKeyAsync(int id);
        Task<string> GetClientPublicRSAKeysAsync(int id);
        Task SaveClientRSAKeyAsync(int id, string key);
    }
}