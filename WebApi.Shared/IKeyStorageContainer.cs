using System.Threading.Tasks;

namespace WebApi.Shared
{
    public interface IKeyStorageContainer
    {
        Task<bool> PublicKeyExists(int id);
        Task<bool> PrivateKeyExists(int id);
        Task<string> ReadPublickKeyAsStringAsync(int id);
        Task<string> ReadPrivateKeyAsStringAsync(int id);
        Task WritePublicKeyAsync(int id, string value);
        Task WritePrivateKeyAsync(int id, string value);
    }
}
