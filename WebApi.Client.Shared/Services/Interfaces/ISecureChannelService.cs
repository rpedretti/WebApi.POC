using System.Threading.Tasks;
using WebApi.Shared.Models;

namespace WebApi.Client.Shared.Services
{
    public interface ISecureChannelService
    {
        Task RequestJwtAsync(UserAuthenticationModel userData, bool forceRefresh);
        Task UpdateJwtAsync();
        Task OpenSecureChannelAsync(string username, string password, bool forceTokenUpdate = false);
        Task PostOnSecureChannelAsync(object message, string url);
        Task<T> PostOnSecureChannelAsync<T>(object message, string url);
        Task<T> GetOnSecureChannelAsync<T>(string url);
    }
}
