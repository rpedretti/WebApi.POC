using MvvmCross.Platform.Platform;
using Newtonsoft.Json;
using System;
using System.IO;

namespace WebApi.UWP.Helpers
{
    public class JsonSerializer : IMvxJsonConverter
    {
        public T DeserializeObject<T>(Stream stream)
        {
            throw new NotImplementedException();
        }

        public T DeserializeObject<T>(string inputText)
        {
            return JsonConvert.DeserializeObject<T>(inputText);
        }

        public object DeserializeObject(Type type, string inputText)
        {
            return JsonConvert.DeserializeObject(inputText, type);
        }

        public string SerializeObject(object toSerialise)
        {
            return JsonConvert.SerializeObject(toSerialise);
        }
    }
}
