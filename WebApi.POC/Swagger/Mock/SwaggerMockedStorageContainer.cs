using System;
using System.Threading.Tasks;
using WebApi.Shared;

namespace WebApi.POC.Swagger.Mock
{
    /// <summary>
    /// Mocked Key Storage to be used at Swagger
    /// </summary>
    public class SwaggerMockedStorageContainer : IKeyStorageContainer
    {

        /// <summary>
        /// Checks is a private key exists for the given id
        /// </summary>
        /// <param name="id">The id to be checked</param>
        /// <returns>
        /// A task wich result is a boolean indicating weather the key exists or not
        /// </returns>
        public Task<bool> PrivateKeyExists(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if a publick key exists for the given id
        /// </summary>
        /// <param name="id">The id to be checked</param>
        /// <returns>
        /// A task wich result is a boolean indicating weather the key exists or not
        /// </returns>
        public Task<bool> PublicKeyExists(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the private key for the given id
        /// </summary>
        /// <param name="id">The id to be read</param>
        /// <returns>
        /// A task wich result is a string of the key
        /// </returns>
        public Task<string> ReadPrivateKeyAsStringAsync(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the public key for the given id
        /// </summary>
        /// <param name="id">The id to be read</param>
        /// <returns>
        /// A task wich result is a string of the key
        /// </returns>
        public Task<string> ReadPublickKeyAsStringAsync(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a private key for the given id
        /// </summary>
        /// <param name="id">The id to be written</param>
        /// <param name="value"></param>
        /// <returns>
        /// A empty task
        /// </returns>
        public Task WritePrivateKeyAsync(string id, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a public key for the given id
        /// </summary>
        /// <param name="id">The id to be written</param>
        /// <param name="value"></param>
        /// <returns>
        /// A empty task
        /// </returns>
        public Task WritePublicKeyAsync(string id, string value)
        {
            throw new NotImplementedException();
        }
    }
}
