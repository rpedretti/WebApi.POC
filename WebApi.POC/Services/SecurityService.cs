using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.POC.Services
{
    public class SecurityService : ISecurityService
    {
        private ICryptoService _cryptoService;
        private IKeyStorageContainer _keyStorageContainer;
        private const string _rsaKeyPath = "./pub_keys";

        public SecurityService(ICryptoService cryptoService, IKeyStorageContainer keyStorageContainer)
        {
            _cryptoService = cryptoService;
            _keyStorageContainer = keyStorageContainer;
        }

        public async Task<string> GetPublicRSAKeyAsync(int id)
        {
            var keys = await GetPublicPrivatRSAKeyAsync(id);
            return keys.Item1;
        }

        public async Task<string> GetPrivateRSAKeyAsync(int id)
        {
            var keys = await GetPublicPrivatRSAKeyAsync(id);
            return keys.Item2;
        }

        public async Task SaveClientRSAKeyAsync(int id, string key)
        {
            await _keyStorageContainer.WritePublicKeyAsync(id, key);
        }

        public async Task<string> GetClientPublicRSAKeysAsync(int id)
        {
            return await _keyStorageContainer.ReadPublickKeyAsStringAsync(id);
        }

        private async Task<Tuple<string, string>> GetPublicPrivatRSAKeyAsync(int id)
        {
            Tuple<string, string> keys;
            if (await _cryptoService.RSAKeysExists(id))
            {
                keys = await _cryptoService.GetRSAKeysFromStorage(id);
            } else {
                keys = await _cryptoService.GenerateRSAKeyPairAsync();
                await _keyStorageContainer.WritePublicKeyAsync(0, keys.Item1);
                await _keyStorageContainer.WritePrivateKeyAsync(0, keys.Item2);
            }

            return keys;
        }
    }
}
