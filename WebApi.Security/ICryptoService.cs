using System;
using System.Threading.Tasks;

namespace WebApi.Security
{
    public interface ICryptoService
    {
        Task<Tuple<string, string>> GenerateRSAKeyPairAsync();
        Task<byte[]> GenerateTripleDESKeyAsync();
        void RegisterMergedKey(string id, byte[] key);
        byte[] GenerateCombinedTripleDesKey(byte[] key1, byte[] key2);
        byte[] RetrieveMergedKey(string id);
        void RemoveMergedKey(string id);

        Task<string> DecryptRSAAsync(byte[] value, string key);
        Task<byte[]> EncryptRSAAsync(string value, string key);
        Task<string> DecryptTripleDESAsync(byte[] value, byte[] key);
        Task<byte[]> EncryptTripleDESAsync(string value, byte[] key);

        string HashWithSha256(string v);
    }
}