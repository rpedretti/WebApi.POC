using System;
using System.IO;
using System.Threading.Tasks;
using WebApi.Shared;

namespace WebApi.Droid.Helpers
{
    public sealed class LocalStorageContainer : IStorageContainer
    {
        public Task<bool> FileExists(string path)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filePath = Path.Combine(documentsPath, path);
            return Task.FromResult(File.Exists(filePath));
        }

        public async Task<string> ReadFileAsStringAsync(string path)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filePath = Path.Combine(documentsPath, path);
            return await Task.Run(() => { return File.ReadAllText(filePath); });
        }

        public async Task WriteFileAsync(string path, string value)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filePath = Path.Combine(documentsPath, path);
            await Task.Run(() => File.WriteAllText(filePath, value));
        }
    }
}
