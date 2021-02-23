using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;
using Newtonsoft.Json;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class DeserializedObject<T> : IDeserializedObject<T> where T: class
    {
        private static string _fileName;

        public ConcurrentDictionary<string, T> ConcurrentDictionary { get; set; }

        public DeserializedObject()
        {
            ConcurrentDictionary = GetData().Result;
        }

        private async Task<ConcurrentDictionary<string, T>> GetData()
        {
           return await Deserialize();
        }
        public async Task UpdateData()
        {
            await Serialize();
        }
        
        private Task<bool> IsNeededFilesAvailable()
        { 
            return Task.Run(()=> File.Exists(_fileName));
        }


        private async Task<ConcurrentDictionary<string, T>> Deserialize()
        {
            _fileName = typeof(T).Name.Contains("Statistics") ? "Statistics" : typeof(T).Name;
            
            var exists = IsNeededFilesAvailable().Result;

            FileStream reader;
            if (exists && File.ReadAllTextAsync(_fileName).Result != "")
            {
                byte[] fileText;
                await using (reader = File.Open(_fileName, FileMode.Open))
                {
                    fileText = new byte[reader.Length];
                    await reader.ReadAsync(fileText, 0, (int) reader.Length);
                }

                var decoded = Encoding.ASCII.GetString(fileText);


                var list = await Task.Run(() =>
                    JsonConvert.DeserializeObject<ConcurrentDictionary<string, T>>(decoded));
                
                return list;
            }
            reader = File.Open(_fileName, FileMode.OpenOrCreate);
            
            reader.Close();
            
            return new ConcurrentDictionary<string, T>();
        }

        private async Task Serialize()
        {
            var streamManager = new RecyclableMemoryStreamManager();

            using var file = File.Open(_fileName, FileMode.OpenOrCreate);
            using var memoryStream = streamManager.GetStream();
            using var writer = new StreamWriter(memoryStream);
            
            var serializer = JsonSerializer.CreateDefault();
                
            serializer.Serialize(writer, ConcurrentDictionary);      // FROM STACKOVERFLOW
                
            await writer.FlushAsync().ConfigureAwait(false);
                
            memoryStream.Seek(0, SeekOrigin.Begin);
                
            await memoryStream.CopyToAsync(file).ConfigureAwait(false);

            await file.FlushAsync().ConfigureAwait(false);
            
        }
    }
}