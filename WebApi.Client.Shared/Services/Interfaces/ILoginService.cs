using System.Threading.Tasks;

namespace WebApi.Client.Shared.Services
{
    public interface ILoginService
    {
        Task LoginAsync(string username, string password);
        Task OpenSecureChannelForLoggedUserAsync();
        bool IsUserLogged();
        Task LogoutAsync();
    }
}
