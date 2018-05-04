using NHibernate;
using NHibernate.Linq;
using System.Threading.Tasks;
using WebApi.POC.Domain;
using WebApi.POC.Repository.Local;
using WebApi.Shared;

namespace WebApi.POC.Utils
{
    /// <summary>
    /// Key container stored at a database
    /// </summary>
    public class DbKeyStorageContainer : BaseNHContext, IKeyStorageContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        public DbKeyStorageContainer(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Checks if a publick key exists for the given id
        /// </summary>
        /// <param name="id">The id to be checked</param>
        /// <returns>A task wich result is a boolean indicating weather the key exists or not</returns>
        public async Task<bool> PublicKeyExists(string id)
        {
            return await Read<CryptoKey>().AnyAsync(c => c.Id == id && c.Kind.Id == KeyKind.PUBLIC.Id);
        }

        /// <summary>
        /// Checks is a private key exists for the given id
        /// </summary>
        /// <param name="id">The id to be checked</param>
        /// <returns>A task wich result is a boolean indicating weather the key exists or not</returns>
        public async Task<bool> PrivateKeyExists(string id)
        {
            return await Read<CryptoKey>().AnyAsync(c => c.Id == id && c.Kind.Id == KeyKind.PRIVATE.Id);
        }

        /// <summary>
        /// Get the public key for the given id
        /// </summary>
        /// <param name="id">The id to be read</param>
        /// <returns>A task wich result is a string of the key</returns>
        public async Task<string> ReadPublickKeyAsStringAsync(string id)
        {
            var key = await Read<CryptoKey>().FirstAsync(c => c.Id == id && c.Kind.Id == KeyKind.PUBLIC.Id);
            return key.Value;
        }

        /// <summary>
        /// Get the private key for the given id
        /// </summary>
        /// <param name="id">The id to be read</param>
        /// <returns>A task wich result is a string of the key</returns>
        public async Task<string> ReadPrivateKeyAsStringAsync(string id)
        {
            var key = await Read<CryptoKey>().FirstAsync(c => c.Id == id && c.Kind.Id == KeyKind.PRIVATE.Id);
            return key.Value;
        }

        /// <summary>
        /// Writes a private key for the given id
        /// </summary>
        /// <param name="id">The id to be written</param>
        /// <param name="value">The value to be written</param>
        /// <returns>
        /// A empty task
        /// </returns>
        public async Task WritePrivateKeyAsync(string id, string value)
        {
            var kind = KeyKind.PRIVATE;
            await InternalWriteKeyAsync(id, value, kind);
        }

        /// <summary>
        /// Writes a public key for the given id
        /// </summary>
        /// <param name="id">The id to be written</param>
        /// <param name="value">The value to be writen</param>
        /// <returns>
        /// A empty task
        /// </returns>
        /// |
        public async Task WritePublicKeyAsync(string id, string value)
        {
            var kind = KeyKind.PUBLIC;
            await InternalWriteKeyAsync(id, value, kind);
        }

        private async Task InternalWriteKeyAsync(string id, string value, KeyKind kind)
        {
            await WithAutoTransaction(async session =>
            {
                var key = await Read<CryptoKey>().FirstOrDefaultAsync(c => c.Id == id && c.Kind.Id == kind.Id);
                if (key == null)
                {
                    var cryptoKey = new CryptoKey
                    {
                        Id = id,
                        Kind = kind,
                        Value = value
                    };
                    await session.SaveAsync(cryptoKey);
                    await session.FlushAsync();
                }
                else
                {
                    key.Value = value;
                    await session.UpdateAsync(key);
                    await session.FlushAsync();
                }
            });
        }
    }
}
