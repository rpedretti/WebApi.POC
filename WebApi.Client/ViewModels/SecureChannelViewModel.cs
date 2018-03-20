
using Newtonsoft.Json;
using Prism.Windows.Mvvm;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Client.Services;
using WebApi.Security;
using WebApi.Shared;
using WebApi.Shared.Models;

namespace WebApi.Client.ViewModels
{
    public class SecureChannelViewModel : ViewModelBase
    {
        private ICryptoService _cryptoService;
        private IStorageContainer _storageContainer;
        private ISecurityService _securityService;
        private const string _rsaKeyPath = "";
        private string _message;
        private string _username;
        private string _password;
        private string _response;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string Response
        {
            get { return _response; }
            set { SetProperty(ref _response, value); }
        }


        public SecureChannelViewModel(ICryptoService cryptoService, IStorageContainer storageContainer, ISecurityService securityService)
        {
            _cryptoService = cryptoService;
            _storageContainer = storageContainer;
            _securityService = securityService;
        }

        public async void RequestSecureChannel()
        {
            try
            {
                var keys = await _cryptoService.RSAKeysExists(_rsaKeyPath) ?
                    await _cryptoService.GetRSAKeysFromStorage(_rsaKeyPath) :
                    await _cryptoService.GenerateRSAKeyPairAsync(_rsaKeyPath);

                // Sends public key and get server's public key in return
                var serverRsaKey = await _securityService.ExchangeRsaKey(keys.Item1);

                // Generates a 3DES key
                var tripleDesKey = await _cryptoService.GenerateTripleDESKeyAsync();

                // Encrypt the 3DES key with server RSA public key
                var encryptedTripleDesKey = await _cryptoService.EncryptRSAAsync(Convert.ToBase64String(tripleDesKey), serverRsaKey.Key);

                // Sends the encrypted key to the server and gets an 3DES key in return
                var serverTripleDesMessage = await _securityService.ExchangeTripleDesKey(Convert.ToBase64String(encryptedTripleDesKey), keys.Item2);

                // Merges both 3DES key to generate a new key used by both sides
                var mergedKey = _cryptoService.GenerateCombinedTripleDesKey(tripleDesKey, Convert.FromBase64String(serverTripleDesMessage.Key));
                _cryptoService.RegisterMergedKey(serverRsaKey.Id, mergedKey);


                var userData = new UserAuthenticationModel()
                {
                    Username = Username,
                    Password = _cryptoService.HashWithSha256(Password)
                };

                await _securityService.RequestJwtAsync(userData);
                Response = "Success";
            }
            catch (Exception e)
            {
                Response = e.Message;
            }
        }

        /// <summary>
        /// Sends a message over a secure channel
        /// </summary>
        public async void SendSecureMessage()
        {
            var userData = new UserAuthenticationModel()
            {
                Username = Username,
                Password = _cryptoService.HashWithSha256(Password)
            };
            Response = await _securityService.SendMessageOnSecureChannel(Message, userData);
        }
    }
}
