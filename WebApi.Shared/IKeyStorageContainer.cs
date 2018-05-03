using System.Threading.Tasks;

namespace WebApi.Shared
{
    /// <summary>
    /// Handle keys requests
    /// </summary>
    public interface IKeyStorageContainer
    {
        /// <summary>
        /// Checks if a publick key exists for the given id
        /// </summary>
        /// <param name="id">The id to be checked</param>
        /// <returns>A task wich result is a boolean indicating weather the key exists or not</returns>
        Task<bool> PublicKeyExists(string id);

        /// <summary>
        /// Checks is a private key exists for the given id
        /// </summary>
        /// <param name="id">The id to be checked</param>
        /// <returns>A task wich result is a boolean indicating weather the key exists or not</returns>
        Task<bool> PrivateKeyExists(string id);

        /// <summary>
        /// Get the public key for the given id
        /// </summary>
        /// <param name="id">The id to be read</param>
        /// <returns>A task wich result is a string of the key</returns>
        Task<string> ReadPublickKeyAsStringAsync(string id);

        /// <summary>
        /// Get the private key for the given id
        /// </summary>
        /// <param name="id">The id to be read</param>
        /// <returns>A task wich result is a string of the key</returns>
        Task<string> ReadPrivateKeyAsStringAsync(string id);

        /// <summary>
        /// Writes a public key for the given id
        /// </summary>
        /// <param name="id">The id to be written</param>
        /// |<param name="value">The value to be writen</param>
        /// <returns>A empty task</returns>
        Task WritePublicKeyAsync(string id, string value);

        /// <summary>
        /// Writes a private key for the given id
        /// </summary>
        /// <param name="id">The id to be written</param>
        /// <param name="value">The value to be written</param>
        /// <returns>A empty task</returns>
        Task WritePrivateKeyAsync(string id, string value);
    }
}
