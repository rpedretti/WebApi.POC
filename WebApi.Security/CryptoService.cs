using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Security
{
    public sealed class CryptoService : ICryptoService
    {
        private static Dictionary<int, byte[]> _mergedTripleDesKeys = new Dictionary<int, byte[]>();
        private RSAService _rsaService = new RSAService();
        private TripleDESService _tripleDESService = new TripleDESService();
        
        public async Task<Tuple<string, string>> GenerateRSAKeyPairAsync()
        {
            var keys = await Task.Run(() => { return _rsaService.GenerateKeyPair(); });
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

        public void RemoveMergedKey(int id)
        {
            _mergedTripleDesKeys.Remove(id);
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
