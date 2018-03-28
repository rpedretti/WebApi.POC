﻿using System;
using System.Threading.Tasks;

namespace WebApi.Security
{
    public interface ICryptoService
    {
        Task<bool> RSAKeysExists(int id);
        Task<Tuple<string, string>> GetRSAKeysFromStorage(int id);
        Task<Tuple<string, string>> GenerateRSAKeyPairAsync();
        Task<byte[]> GenerateTripleDESKeyAsync();
        void RegisterMergedKey(int id, byte[] key);
        byte[] GenerateCombinedTripleDesKey(byte[] key1, byte[] key2);
        byte[] RetrieveMergedKey(int id);

        Task<string> DecryptRSAAsync(byte[] value, string key);
        Task<byte[]> EncryptRSAAsync(string value, string key);
        Task<string> DecryptTripleDESAsync(byte[] value, byte[] key);
        Task<byte[]> EncryptTripleDESAsync(string value, byte[] key);

        string HashWithSha256(string v);
    }
}