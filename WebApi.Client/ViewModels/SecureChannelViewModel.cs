
using Prism.Windows.Mvvm;
using System;
using System.Threading.Tasks;
using WebApi.Client.Services;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.Client.ViewModels
{
    public class SecureChannelViewModel : ViewModelBase
    {
        private ICryptoService _cryptoService;
        private IStorageContainer _storageContainer;
        private ISecurityService _securityService;
        private const string _rsaKeyPath = "";
        private string _message;
        private string _response;

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
            var keys = await _cryptoService.RSAKeysExists(_rsaKeyPath) ?
                await _cryptoService.GetRSAKeysFromStorage(_rsaKeyPath) :
                await _cryptoService.GenerateRSAKeyPairAsync(_rsaKeyPath);

            var serverRsaKey = await _securityService.ExchangeRsaKey(keys.Item1);
            var tripleDesKey = await _cryptoService.GenerateTripleDESKeyAsync();
            var encryptedTripleDesKey = await _cryptoService.EncryptRSAAsync(Convert.ToBase64String(tripleDesKey), serverRsaKey.Key);

            var serverTripleDesMessage = await _securityService.ExchangeTripleDesKey(Convert.ToBase64String(encryptedTripleDesKey), keys.Item2);

            var mergedKey = _cryptoService.GenerateCombinedTripleDesKey(tripleDesKey, Convert.FromBase64String(serverTripleDesMessage.Key));
            _cryptoService.RegisterMergedKey(serverRsaKey.Id, mergedKey);

            System.Diagnostics.Debug.WriteLine("merged key: " + Convert.ToBase64String(mergedKey));

        }

        public async void SendSecureMessage()
        {
            var response = await _securityService.SendMessageOnSecureChannel(Message);
            Response = response;
        }
    }
}
