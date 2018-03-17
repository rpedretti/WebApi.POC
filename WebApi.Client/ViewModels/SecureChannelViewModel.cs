
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

            var serverTripleDesKey = await _securityService.ExchangeTripleDesKey(Convert.ToBase64String(encryptedTripleDesKey));
            var decryptedServerTripleDesKey = await _cryptoService.DecryptRSAAsync(Convert.FromBase64String(serverTripleDesKey.Key), keys.Item2);

            var mergedKey = _cryptoService.GenerateCombinedTripleDesKey(tripleDesKey, Convert.FromBase64String(decryptedServerTripleDesKey));
            _cryptoService.RegisterMergedKey(serverRsaKey.Id, mergedKey);

            System.Diagnostics.Debug.WriteLine("merged key: " + Convert.ToBase64String(mergedKey));

        }
    }
}
