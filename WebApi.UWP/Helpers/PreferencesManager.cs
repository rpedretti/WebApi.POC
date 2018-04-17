using Newtonsoft.Json;
using WebApi.Shared;
using Windows.Storage;

namespace WebApi.UWP.Helpers
{
    public sealed class PreferencesManager : IPreferencesManager
    {
        private ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        public T Get<T>(string key)
        {
            _settings.Values.TryGetValue(key, out object value);
            return value != null ? JsonConvert.DeserializeObject<T>(value as string) : default(T);
        }

        public void Set<T>(string key, T value)
        {
            _settings.Values[key] = JsonConvert.SerializeObject(value);
        }
    }
}
