﻿using System;
using System.Threading.Tasks;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.POC.Services
{
    /// <summary>
    /// Handles all request that needs security
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private ICryptoService _cryptoService;
        private IKeyStorageContainer _keyStorageContainer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cryptoService">Intance of the crypto service</param>
        /// <param name="keyStorageContainer">Instance of the key storage container</param>
        public SecurityService(ICryptoService cryptoService, IKeyStorageContainer keyStorageContainer)
        {
            _cryptoService = cryptoService;
            _keyStorageContainer = keyStorageContainer;
        }

        /// <summary>
        /// Gets the public key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <returns>The key as string</returns>
        public async Task<string> GetPublicRSAKeyAsync(string id)
        {
            var keys = await GetPublicPrivateRSAKeyAsync(id);
            return keys.Item1;
        }

        /// <summary>
        /// Gets the private key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <returns>The key as string</returns>
        public async Task<string> GetPrivateRSAKeyAsync(string id)
        {
            var keys = await GetPublicPrivateRSAKeyAsync(id);
            return keys.Item2;
        }

        /// <summary>
        /// Gets the public key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <returns>The key as string</returns>
        public async Task<string> GetClientPublicRSAKeysAsync(string id)
        {
            return await _keyStorageContainer.ReadPublickKeyAsStringAsync(id);
        }

        /// <summary>
        /// Saves a public key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <param name="key">The key to be saved</param>
        /// <returns>The key as string</returns>
        public async Task SaveClientRSAKeyAsync(string id, string key)
        {
            await _keyStorageContainer.WritePublicKeyAsync(id, key);
        }

        private async Task<Tuple<string, string>> GetPublicPrivateRSAKeyAsync(string id)
        {
            Tuple<string, string> keys;
            if (await RSAKeysExists(id))
            {
                keys = await GetRSAKeysFromStorage(id);
            } else {
                keys = await _cryptoService.GenerateRSAKeyPairAsync();
                await _keyStorageContainer.WritePublicKeyAsync(id, keys.Item1);
                await _keyStorageContainer.WritePrivateKeyAsync(id, keys.Item2);
            }

            return keys;
        }

        private async Task<bool> RSAKeysExists(string id)
        {
            return await _keyStorageContainer.PublicKeyExists(id)
                && await _keyStorageContainer.PrivateKeyExists(id);
        }

        private async Task<Tuple<string, string>> GetRSAKeysFromStorage(string id)
        {
            var publicKey = await _keyStorageContainer.ReadPublickKeyAsStringAsync(id);
            var publicPrvateKey = await _keyStorageContainer.ReadPrivateKeyAsStringAsync(id);

            return Tuple.Create(publicKey, publicPrvateKey);
        }
    }
}
