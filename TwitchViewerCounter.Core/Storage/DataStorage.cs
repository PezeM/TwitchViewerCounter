using Newtonsoft.Json;
using System.IO;
using TwitchViewerCounter.Core.Constans;
using TwitchViewerCounter.Core.Exceptions;

namespace TwitchViewerCounter.Core.Storage
{
    public class DataStorage : IDataStorage
    {
        public DataStorage()
        {
            if (!Directory.Exists(Globals.DataStoragePath))
            {
                Directory.CreateDirectory(Globals.DataStoragePath);
            }
        }

        public T RestoreObject<T>(string fileName)
        {
            var filePath = GetJsonFilePath(fileName);
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (FileNotFoundException)
            {
                throw new DataStorageFileDoesNotExistsException($"File {fileName} does not exists.");
            }
        }

        public void StoreObject(object obj, string fileName)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var filePath = GetJsonFilePath(fileName);

            File.WriteAllText(filePath, json);
        }

        private string GetJsonFilePath(string fileName)
        {
            return Path.Combine(Globals.DataStoragePath, $"{fileName}.json");
        }
    }
}
