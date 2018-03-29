using System;
using System.Threading.Tasks;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.POC.Services
{
    public class SecurityService : ISecurityService
    {
        private ICryptoService _cryptoService;
        private IKeyStorageContainer _keyStorageContainer;

        public SecurityService(ICryptoService cryptoService, IKeyStorageContainer keyStorageContainer)
        {
            _cryptoService = cryptoService;
            _keyStorageContainer = keyStorageContainer;
        }

        public async Task<string> GetPublicRSAKeyAsync(int id)
        {
            var keys = await GetPublicPrivateRSAKeyAsync(id);
            return keys.Item1;
        }

        public async Task<string> GetPrivateRSAKeyAsync(int id)
        {
            var keys = await GetPublicPrivateRSAKeyAsync(id);
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

        private async Task<Tuple<string, string>> GetPublicPrivateRSAKeyAsync(int id)
        {
            Tuple<string, string> keys;
            if (await RSAKeysExists(id))
            {
                keys = await GetRSAKeysFromStorage(id);
            } else {
                keys = await _cryptoService.GenerateRSAKeyPairAsync();
                await _keyStorageContainer.WritePublicKeyAsync(0, keys.Item1);
                await _keyStorageContainer.WritePrivateKeyAsync(0, keys.Item2);
            }

            return keys;
        }

        private async Task<bool> RSAKeysExists(int id)
        {
            return await _keyStorageContainer.PublicKeyExists(id)
                && await _keyStorageContainer.PrivateKeyExists(id);
        }

        private async Task<Tuple<string, string>> GetRSAKeysFromStorage(int id)
        {
            var publicKey = await _keyStorageContainer.ReadPublickKeyAsStringAsync(id);
            var publicPrvateKey = await _keyStorageContainer.ReadPrivateKeyAsStringAsync(id);

            return Tuple.Create(publicKey, publicPrvateKey);
        }
    }
}
