using System;
using System.Threading.Tasks;

namespace WebApi.Security
{
    public interface ICryptoService
    {
        Task<bool> RSAKeysExists(string path);
        Task<string> DecryptRSAAsync(byte[] value, string key);
        Task<string> DecryptTripleDESAsync(byte[] value, byte[] key);
        Task<byte[]> EncryptRSAAsync(string value, string key);
        Task<byte[]> EncryptTripleDESAsync(string value, byte[] key);
        Task<Tuple<string, string>> GetRSAKeysFromStorage(string path);
        Task<Tuple<string, string>> GenerateRSAKeyPairAsync(string savePath);
        Task<byte[]> GenerateTripleDESKeyAsync();
        byte[] GenerateCombinedTripleDesKey(byte[] key1, byte[] key2);
        void RegisterMergedKey(int id, byte[] key);
        byte[] RetrieveMergedKey(int id);
        string HashWithSha256(string v);
    }
}