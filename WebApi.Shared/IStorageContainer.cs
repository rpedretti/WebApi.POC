using System.Threading.Tasks;

namespace WebApi.Shared
{
    /// <summary>
    /// Handle all file requests
    /// </summary>
    public interface IStorageContainer
    {
        /// <summary>
        /// Checks if a file exists asynchronously.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a boolean indicating if the file exists</returns>
        Task<bool> FileExistsAsync(string path);

        /// <summary>
        /// Reads the file as string asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Task<string> ReadFileAsStringAsync(string path);

        /// <summary>
        /// Writes the file asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task WriteFileAsync(string path, string value);
    }
}
