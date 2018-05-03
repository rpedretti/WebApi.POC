using System;
using System.IO;
using System.Threading.Tasks;
using WebApi.Shared;
using Windows.Storage;

namespace WebApi.UWP.Helpers
{
    public sealed class LocalStorageContainer : IStorageContainer
    {
        public Task<bool> FileExistsAsync(string path)
        {
            var folder = ApplicationData.Current.LocalFolder;
            return Task.FromResult(File.Exists(Path.Combine(folder.Path,path)));
        }

        public async Task<string> ReadFileAsStringAsync(string path)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var dirPath = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var dir = string.IsNullOrWhiteSpace(dirPath) ? folder : await folder.GetFolderAsync(dirPath);
            var file = await dir.GetFileAsync(fileName);
            return await FileIO.ReadTextAsync(file);
        }

        public async Task WriteFileAsync(string path, string value)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var dirPath = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var dir = string.IsNullOrWhiteSpace(dirPath) ? folder : await folder.CreateFolderAsync(dirPath, CreationCollisionOption.OpenIfExists);
            var file = await dir.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, value);
        }
    }
}
