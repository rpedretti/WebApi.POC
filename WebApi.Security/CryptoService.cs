using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApi.Shared;

namespace WebApi.Security
{
    public sealed class CryptoService : ICryptoService
    {
        private static Dictionary<int, byte[]> _mergedTripleDesKeys = new Dictionary<int, byte[]>();
        private RSAService _rsaService = new RSAService();
        private TripleDESService _tripleDESService = new TripleDESService();
        private IStorageContainer _storageContainer;

        public CryptoService(IStorageContainer storageContainer)
        {
            _storageContainer = storageContainer;
        }

        public async Task<bool> RSAKeysExists(string path)
        {
            return await _storageContainer.FileExists(path + "key.pub")
                && await _storageContainer.FileExists(path + "key.priv");
        }

        public async Task<Tuple<string, string>> GetRSAKeysFromStorage(string path)
        {
            var publicKey = await _storageContainer.ReadFileAsStringAsync(path + "key.pub");
            var publicPrvateKey = await _storageContainer.ReadFileAsStringAsync(path + "key.priv");

            return Tuple.Create(publicKey, publicPrvateKey);
        }

        public async Task<Tuple<string, string>> GenerateRSAKeyPairAsync(string savePath)
        {
            var keys = await Task.Run(() => { return _rsaService.GenerateKeyPair(); });
            await _storageContainer.WriteFileAsync(savePath + "key.pub", keys.Item1);
            await _storageContainer.WriteFileAsync(savePath + "key.priv", keys.Item2);
            return keys;
        }

        public async Task<byte[]> EncryptRSAAsync(string value, string key)
        {
            return await Task.Run(() => { return _rsaService.Encrypt(value, key); });
        }

        public async Task<string> DecryptRSAAsync(byte[] value, string key)
        {
            return await Task.Run(() => { return _rsaService.Decrypt(value, key); });
        }

        public async Task<byte[]> GenerateTripleDESKeyAsync()
        {
            return await Task.Run(() => { return _tripleDESService.GenerateKey(); });
        }

        public async Task<byte[]> EncryptTripleDESAsync(string value, byte[] key)
        {
            return await Task.Run(() => { return _tripleDESService.Encrypt(value, key); });
        }

        public async Task<string> DecryptTripleDESAsync(byte[] value, byte[] key)
        {
            return await Task.Run(() => { return _tripleDESService.Decrypt(value, key); });
        }

        public byte[] GenerateCombinedTripleDesKey(byte[] key1, byte[] key2)
        {
            byte[] mergedKey = new byte[key1.Length];

            for (int i = 0; i < mergedKey.Length; i++)
            {
                mergedKey[i] = (byte)(key1[i] & key2[i]);
            }

            return mergedKey;
        }

        public void RegisterMergedKey(int id, byte[] key)
        {
            _mergedTripleDesKeys[id] = key;
        }

        public byte[] RetrieveMergedKey(int id)
        {
            return _mergedTripleDesKeys[id];
        }

        public string HashWithSha256(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder();
                for (var i = 0; i < hashed.Length; i++)
                {
                    sb.Append(String.Format("{0:X2}", hashed[i]));
                }
                return sb.ToString();
            }
        }
    }
}
