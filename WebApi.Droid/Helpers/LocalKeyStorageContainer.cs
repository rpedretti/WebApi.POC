using System.Threading.Tasks;
using WebApi.Shared;
using WebApi.Security;

namespace WebApi.Droid.Helpers
{
    public sealed class LocalKeyStorageContainer : IKeyStorageContainer
    {
        private IStorageContainer _storageContainer;

        public LocalKeyStorageContainer(IStorageContainer storageContainer)
        {
            _storageContainer = storageContainer;
        }

        public async Task<bool> PrivateKeyExists(int id)
        {
            return await _storageContainer.FileExists($"{id}/key.priv");
        }

        public async Task<bool> PublicKeyExists(int id)
        {
            return await _storageContainer.FileExists($"{id}/key.pub");
        }

        public async Task<string> ReadPrivateKeyAsStringAsync(int id)
        {
            return await _storageContainer.ReadFileAsStringAsync($"{id}/key.priv");
        }

        public async Task<string> ReadPublickKeyAsStringAsync(int id)
        {
            return await _storageContainer.ReadFileAsStringAsync($"{id}/key.pub");
        }

        public async Task WritePrivateKeyAsync(int id, string value)
        {
            await _storageContainer.WriteFileAsync($"{id}/key.priv", value);
        }

        public async Task WritePublicKeyAsync(int id, string value)
        {
            await _storageContainer.WriteFileAsync($"{id}/key.pub", value);
        }
    }
}