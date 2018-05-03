using System.Threading.Tasks;

namespace WebApi.POC.Services
{
    /// <summary>
    /// Handles all security demands
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Gets the public key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <returns>The key as string</returns>
        Task<string> GetPublicRSAKeyAsync(string id);

        /// <summary>
        /// Gets the private key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <returns>The key as string</returns>
        Task<string> GetPrivateRSAKeyAsync(string id);

        /// <summary>
        /// Gets the public key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <returns>The key as string</returns>
        Task<string> GetClientPublicRSAKeysAsync(string id);

        /// <summary>
        /// Saves a public key for the given id
        /// </summary>
        /// <param name="id">The key owner id</param>
        /// <param name="key">The key to be saved</param>
        /// <returns>The key as string</returns>
        Task SaveClientRSAKeyAsync(string id, string key);
    }
}