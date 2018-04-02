
using Android.App;
using Android.Content;
using Android.Preferences;
using Newtonsoft.Json;
using WebApi.Shared;

namespace WebApi.Droid.Helpers
{
    public sealed class PreferencesManager : IPreferencesManager
    {
        private Context context;

        public PreferencesManager()
        {
            context = Application.Context;
        }

        public T Get<T>(string key)
        {
            var manager = PreferenceManager.GetDefaultSharedPreferences(context);
            var stored = manager.GetString(key, null);
            var value = stored != null ? JsonConvert.DeserializeObject<T>(stored) : default(T);
            return value;
        }

        public void Set<T>(string key, T value)
        {
            var manager = PreferenceManager.GetDefaultSharedPreferences(context);
            using (var editor = manager.Edit())
            {
                var serialized = JsonConvert.SerializeObject(value);
                editor.PutString(key, serialized);
                editor.Commit();
            }
        }
    }
}