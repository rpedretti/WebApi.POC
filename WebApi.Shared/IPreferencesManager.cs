namespace WebApi.Shared
{
    public interface IPreferencesManager
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }
}
