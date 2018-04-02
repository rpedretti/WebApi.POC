using System.Threading.Tasks;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.Client.Shared.Services
{
    public sealed class LoginService : ILoginService
    {
        private readonly ISecureChannelService _securityService;
        private readonly IPreferencesManager _preferencesManager;
        private readonly ICryptoService _cryptoService;

        public LoginService(
            ISecureChannelService securityService,
            IPreferencesManager preferencesManager,
            ICryptoService cryptoService)
        {
            _securityService = securityService;
            _preferencesManager = preferencesManager;
            _cryptoService = cryptoService;
        }

        public bool IsUserLogged()
        {
            return _preferencesManager.Get<bool>("logged");
        }

        public async Task LoginAsync(string username, string password)
        {
            var cryptoPassword = _cryptoService.HashWithSha256(password);
            await _securityService.OpenSecureChannelAsync(username, cryptoPassword, true);
            _preferencesManager.Set("logged", true);
            _preferencesManager.Set("username", username);
            _preferencesManager.Set("password", cryptoPassword);
        }

        public async Task OpenSecureChannelForLoggedUserAsync()
        {
            var username = _preferencesManager.Get<string>("username");
            var password = _preferencesManager.Get<string>("password");
            await _securityService.OpenSecureChannelAsync(username, password);
        }

        public void Logout()
        {
            _preferencesManager.Set("logged", false);
            _preferencesManager.Set("username", "");
            _preferencesManager.Set("password", "");
        }
    }
}
