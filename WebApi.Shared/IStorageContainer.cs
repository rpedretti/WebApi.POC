using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Shared
{
    public interface IStorageContainer
    {
        Task<bool> FileExists(string path);
        Task<string> ReadFileAsStringAsync(string path);
        Task WriteFileAsync(string path, string value);
    }
}
