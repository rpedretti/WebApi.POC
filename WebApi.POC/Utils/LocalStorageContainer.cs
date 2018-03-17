using System.IO;
using System.Threading.Tasks;
using WebApi.Shared;

namespace WebApi.POC.Utils
{
    public class LocalStorageContainer : IStorageContainer
    {
        public Task<bool> FileExists(string path)
        {
            return Task.FromResult(File.Exists(path));
        }

        public async Task<string> ReadFileAsStringAsync(string path)
        {
            var text = await File.ReadAllTextAsync(path);
            return text;
        }

        public async Task WriteFileAsync(string path, string value)
        {
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            await File.WriteAllTextAsync(path, value);
        }
    }
}
