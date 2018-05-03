namespace WebApi.Shared
{
    /// <summary>
    /// Handles all Preferences request
    /// </summary>
    public interface IPreferencesManager
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">The type stored at the key</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>the value associated with the given key</returns>
        T Get<T>(string key);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T">The type that will be stored at the key</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to be stored.</param>
        void Set<T>(string key, T value);
    }
}
