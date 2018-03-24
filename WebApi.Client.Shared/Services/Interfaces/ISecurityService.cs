using System.Threading.Tasks;
using WebApi.Shared.Models;

namespace WebApi.Client.Shared.Services
{
    public interface ISecurityService
    {
        Task<ExchangePublicKeyModel> ExchangeRsaKey(string key);
        Task<ExchangePublicKeyModel> ExchangeTripleDesKey(string key, string rsaKey);
        Task<string> SendMessageOnSecureChannelAsync(object message, string url);
        Task RequestJwtAsync(UserAuthenticationModel userData, bool forceRefresh);
        Task UpdateJwtAsync(UserAuthenticationModel userData);
        Task OpenSecureChannelAsync(string username, string password, bool forceTokenUpdate = false);
    }
}
