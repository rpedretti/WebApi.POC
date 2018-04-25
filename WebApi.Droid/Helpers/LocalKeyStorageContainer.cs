using System.Threading.Tasks;
using WebApi.Shared;

namespace WebApi.Droid.Helpers
{
    public sealed class LocalKeyStorageContainer : IKeyStorageContainer
    {
        private IStorageContainer _storageContainer;

        public LocalKeyStorageContainer(IStorageContainer storageContainer)
        {
            _storageContainer = storageContainer;
        }

        public async Task<bool> PrivateKeyExists(string id)
        {
            return await _storageContainer.FileExists($"{id}/key.priv");
        }

        public async Task<bool> PublicKeyExists(string id)
        {
            return await _storageContainer.FileExists($"{id}/key.pub");
        }

        public async Task<string> ReadPrivateKeyAsStringAsync(string id)
        {
            return await _storageContainer.ReadFileAsStringAsync($"{id}/key.priv");
        }

        public async Task<string> ReadPublickKeyAsStringAsync(string id)
        {
            return await _storageContainer.ReadFileAsStringAsync($"{id}/key.pub");
        }

        public async Task WritePrivateKeyAsync(string id, string value)
        {
            await _storageContainer.WriteFileAsync($"{id}/key.priv", value);
        }

        public async Task WritePublicKeyAsync(string id, string value)
        {
            await _storageContainer.WriteFileAsync($"{id}/key.pub", value);
        }
    }
}