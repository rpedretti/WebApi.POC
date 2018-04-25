using System.IO;
using System.Threading.Tasks;
using WebApi.Shared;

namespace WebApi.POC.Utils
{
    public class LocalKeyStorageContainer : IKeyStorageContainer
    {
        private const string _rsaKeyPath = "./keys";

        public async Task<bool> PrivateKeyExists(string id)
        {
            return await Task.Run(() => File.Exists($"{_rsaKeyPath}/{id}/key.priv"));
        }

        public async Task<bool> PublicKeyExists(string id)
        {
            return await Task.Run(() => File.Exists($"{_rsaKeyPath}/{id}/key.pub"));
        }

        public async Task<string> ReadPrivateKeyAsStringAsync(string id)
        {
            return await File.ReadAllTextAsync($"{_rsaKeyPath}/{id}/key.priv");
        }

        public async Task<string> ReadPublickKeyAsStringAsync(string id)
        {
            return await File.ReadAllTextAsync($"{_rsaKeyPath}/{id}/key.pub");
        }

        public async Task WritePrivateKeyAsync(string id, string value)
        {
            var path = $"{_rsaKeyPath}/{id}/key.priv";
            await InternalWriteAsync(value, path);
        }

        public async Task WritePublicKeyAsync(string id, string value)
        {
            var path = $"{_rsaKeyPath}/{id}/key.pub";
            await InternalWriteAsync(value, path);
        }

        private static async Task InternalWriteAsync(string value, string path)
        {
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            await File.WriteAllTextAsync(path, value);
        }
    }
}
