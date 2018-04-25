using System.Threading.Tasks;

namespace WebApi.Shared
{
    public interface IKeyStorageContainer
    {
        Task<bool> PublicKeyExists(string id);
        Task<bool> PrivateKeyExists(string id);
        Task<string> ReadPublickKeyAsStringAsync(string id);
        Task<string> ReadPrivateKeyAsStringAsync(string id);
        Task WritePublicKeyAsync(string id, string value);
        Task WritePrivateKeyAsync(string id, string value);
    }
}
