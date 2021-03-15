using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class DeserializedObject<T> : IDeserializedObject<T> where T: class
    {
        private static string _fileName = "";

        /// <summary>
        /// Concurrent Dictionary. Heart of our project
        /// </summary>
        public ConcurrentDictionary<string, T> ConcurrentDictionary { get; set; }

        public DeserializedObject()
        {
            ConcurrentDictionary = GetData().Result;
        }

        /// <summary>
        /// Method to retrieve data from Dictionary
        /// </summary>
        /// <returns>ConcurrentDictionary</returns>
        private async Task<ConcurrentDictionary<string, T>> GetData()
        {
           return await Deserialize();
        }
        
        /// <summary>
        /// Method to update data in ConcurrentDictionary
        /// </summary>
        /// <returns></returns>
        public async Task UpdateData()
        {
            //await Serialize();
        }
        
        /// <summary>
        /// Check if file is available. Returns true if it exists. Else false
        /// </summary>
        /// <returns>bool</returns>
        private static Task<bool> IsNeededFilesAvailable()
        { 
            return Task.Factory.StartNew(()=> File.Exists(_fileName));
        }
        
        /// <summary>
        /// Method to deserialize data from T file
        /// </summary>
        /// <returns>ConcurrentDictionary</returns>
        private static async Task<ConcurrentDictionary<string, T>> Deserialize()
        {
            _fileName = typeof(T).Name.Contains("Statistics") ? "Statistics" : typeof(T).Name;

            var exists = IsNeededFilesAvailable().Result;

            FileStream reader;
            if (exists && File.ReadAllTextAsync(_fileName).Result != "")  //todo*/
                try
                {
                    byte[] fileText;
                    await using (reader = File.Open(_fileName, FileMode.Open))
                    {
                        fileText = new byte[reader.Length];
                        await reader.ReadAsync(fileText, 0, (int)reader.Length);
                    }

                    var decoded = Encoding.ASCII.GetString(fileText);
                    
                    
                    var list = await Task.Run(() =>
                        JsonSerializer.Deserialize<ConcurrentDictionary<string,T>>(decoded));
                    return list;
                }
                catch (FileNotFoundException exception)
                {
                    File.Create(_fileName);
                    return new ConcurrentDictionary<string, T>();
                }

            reader = File.Open(_fileName, FileMode.OpenOrCreate);
            reader.Close();
            return new ConcurrentDictionary<string, T>();
            
        }

        /// <summary>
        /// Method to serialize T objects
        /// </summary>
        /// <returns>Void</returns>
        /*private async Task Serialize()
        {
            var streamManager = new RecyclableMemoryStreamManager();

            using var file = File.Open(_fileName, FileMode.OpenOrCreate);
            using var memoryStream = streamManager.GetStream();
            using var writer = new StreamWriter(memoryStream);

            //var serializer = JsonSerializer;
                
            var serializer = JsonSerializer.Serialize(writer, ConcurrentDictionary);      // FROM STACKOVERFLOW
                
            await writer.FlushAsync().ConfigureAwait(false);
                
            memoryStream.Seek(0, SeekOrigin.Begin);
                
            await memoryStream.CopyToAsync(file).ConfigureAwait(false);

            await file.FlushAsync().ConfigureAwait(false);
            
        }*/
    }
}