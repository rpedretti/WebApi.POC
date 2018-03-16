﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Shared;
using Windows.Storage;

namespace WebApi.Client.Helpers
{
    public sealed class LocalStorageContainer : IStorageContainer
    {
        public Task<bool> FileExists(string path)
        {
            return Task.FromResult(File.Exists(path));
        }

        public async Task<string> ReadFileAsStringAsync(string path)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(path);
            return await FileIO.ReadTextAsync(file);
        }

        public async Task WriteFileAsync(string path, string value)
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, value);
        }
    }
}
