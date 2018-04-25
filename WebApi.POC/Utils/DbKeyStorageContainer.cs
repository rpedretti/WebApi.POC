using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.POC.Domain;
using WebApi.POC.Repository;
using WebApi.Shared;

namespace WebApi.POC.Utils
{
    public class DbKeyStorageContainer : IKeyStorageContainer
    {
        private PocDbContext _context;

        public DbKeyStorageContainer(PocDbContext context)
        {
            _context = context;
        }

        public async Task<bool> PrivateKeyExists(string id)
        {
            return await _context.CryptoKeys.AnyAsync(c => c.Id == id && c.KindId == KeyKind.PRIVATE.Id);
        }

        public async Task<bool> PublicKeyExists(string id)
        {
            return await _context.CryptoKeys.AnyAsync(c => c.Id == id && c.KindId == KeyKind.PUBLIC.Id);
        }

        public async Task<string> ReadPrivateKeyAsStringAsync(string id)
        {
            var key = await _context.CryptoKeys.FirstAsync(c => c.Id == id && c.KindId == KeyKind.PRIVATE.Id);
            return key.Value;
        }

        public async Task<string> ReadPublickKeyAsStringAsync(string id)
        {
            var key = await _context.CryptoKeys.FirstAsync(c => c.Id == id && c.KindId == KeyKind.PUBLIC.Id);
            return key.Value;
        }

        public async Task WritePrivateKeyAsync(string id, string value)
        {
            var kind = KeyKind.PRIVATE;
            await InternalWriteKeyAsync(id, value, kind);
        }

        
        public async Task WritePublicKeyAsync(string id, string value)
        {
            var kind = KeyKind.PUBLIC;
            await InternalWriteKeyAsync(id, value, kind);
        }

        private async Task InternalWriteKeyAsync(string id, string value, KeyKind kind)
        {
            var key = await _context.CryptoKeys.FirstOrDefaultAsync(c => c.Id == id && c.KindId == kind.Id);
            if (key == null)
            {
                var cryptoKey = new CryptoKey
                {
                    Id = id,
                    KindId = kind.Id,
                    Value = value
                };
                _context.Add(cryptoKey);
            }
            else
            {
                key.Value = value;
                _context.Update(key);
            }

            await _context.SaveChangesAsync();
        }
    }
}
