using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Shared;

namespace WebApi.Security
{
    public sealed class CryptoService : ICryptoService
    {
        private RSAService rsaService = new RSAService();
        private TripleDESService tripleDESService = new TripleDESService();
        private IStorageContainer _storageContainer;

        public CryptoService(IStorageContainer storageContainer)
        {
            _storageContainer = storageContainer;
        }

        public async Task<bool> RSAKeysExists(string path)
        {
            return await _storageContainer.FileExists(path + "/key.pub")
                && await _storageContainer.FileExists(path + "/key.priv");
        }

        public async Task<Tuple<string, string>> GetRSAKeysFromStrage(string path)
        {
            var publicKey = await _storageContainer.ReadFileAsStringAsync(path + "/key.pub");
            var publicPrvateKey = await _storageContainer.ReadFileAsStringAsync(path + "/key.priv");

            return Tuple.Create(publicKey, publicPrvateKey);
        }

        public async Task<Tuple<string, string>> GenerateRSAKeyPairAsync(string savePath)
        {
            var keys = await Task.Run(() => { return rsaService.GenerateKeyPair(); });
            await _storageContainer.WriteFileAsync(savePath + "key.pub", keys.Item1);
            await _storageContainer.WriteFileAsync(savePath + "key.priv", keys.Item2);
            return keys;
        }

        public async Task<byte[]> EncryptRSAAsync(string value, string key)
        {
            return await Task.Run(() => { return rsaService.Encrypt(value, key); });
        }

        public async Task<string> DecryptRSAAsync(byte[] value, string key)
        {
            return await Task.Run(() => { return rsaService.Decrypt(value, key); });
        }

        public async Task<byte[]> GenerateTripleDESKeyAsync()
        {
            return await Task.Run(() => { return tripleDESService.GenerateKey(); });
        }

        public async Task<byte[]> EncryptTripleDESAsync(string value, byte[] key)
        {
            return await Task.Run(() => { return tripleDESService.Encrypt(value, key); });
        }

        public async Task<string> DecryptTripleDESAsync(byte[] value, byte[] key)
        {
            return await Task.Run(() => { return tripleDESService.Decrypt(value, key); });
        }
    }
}
